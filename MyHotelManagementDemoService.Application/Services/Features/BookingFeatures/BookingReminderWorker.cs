using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.BookingFeatures.Queries
{
    public class BookingReminderWorker : BackgroundService
    {
        private readonly ILogger<BookingReminderWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public BookingReminderWorker(IServiceProvider serviceProvider, ILogger<BookingReminderWorker> logger, IHttpClientFactory httpClientFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessBookingRemindersAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Check daily
            }
        }

        private async Task ProcessBookingRemindersAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var twoDaysFromNow = DateTime.Now.AddDays(2).Date;
            var oneDayFromNow = DateTime.Now.AddDays(1).Date;

            var upcomingBookings = await unitOfWork.bookingRepository.GetWhereAndIncludeAsync(
                b => b.CheckInDate.Date == twoDaysFromNow || b.CheckInDate.Date == oneDayFromNow,
                include: b => b.Include(x => x.User).Include(x => x.Room));

            _logger.LogInformation($"Upcoming bookings count: {(upcomingBookings != null ? upcomingBookings.Count() : 0)}");

            if (upcomingBookings is null)
            {
                _logger.LogWarning("No upcoming bookings found.");
                return;
            }

            foreach (var booking in upcomingBookings)
            {
                try
                {
                    if (booking.CheckInDate.Date == twoDaysFromNow)
                    {
                        await SendTwoDaysReminderAsync(booking);
                        _logger.LogInformation($"Sent Two-days reminder for booking ID: {booking.Id}");
                    }
                    else if (booking.CheckInDate.Date == oneDayFromNow)
                    {
                        await SendOneDayReminderAsync(booking);
                        _logger.LogInformation($"Sent one-day reminder for booking ID: {booking.Id}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error sending reminder for booking ID: {booking.Id}");
                }
            }
        }

        private async Task<bool> SendTwoDaysReminderAsync(Booking booking)
        {
            var request = new SendReminderEmail
            {
                UserEmail = booking.User.Email,
                FirstName = booking.User.FirstName,
                checkInDate = booking.CheckInDate,
                BookingId = booking.Id
            };

            var emailModel = new
            {
                To = request.UserEmail,
                Subject = "Two Days to Check-in Reminder",
                Body = $"Hello {request.FirstName}, this is a reminder that your check-in-date is in 2 days, on {request.checkInDate.Date}."
            };

            return await SendEmailAsync(emailModel);
        }

        private async Task<bool> SendOneDayReminderAsync(Booking booking)
        {
            var request = new SendReminderEmail
            {
                UserEmail = booking.User.Email,
                FirstName = booking.User.FirstName,
                checkInDate = booking.CheckInDate,
                BookingId = booking.Id
            };

            var emailModel = new
            {
                To = request.UserEmail,
                Subject = "One Day to Check-in Reminder",
                Body = $"Hello {request.FirstName}, this is a reminder that your check-in-date  is tomorrow, {request.checkInDate.Date}."
            };

            return await SendEmailAsync(emailModel);
        }
        private async Task<bool> SendEmailAsync(object emailModel)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var sendEmail = await httpClient.PostAsJsonAsync("https://localhost:7168/api/EmailService", emailModel);

            if (sendEmail.IsSuccessStatusCode)
            {
                return true;
            }

            _logger.LogError($"Error sending reminder email");
            return false;
        }
    }

    public class SendReminderEmail
    {
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public DateTime checkInDate { get; set; }
        public int BookingId { get; set; }
    }
}
