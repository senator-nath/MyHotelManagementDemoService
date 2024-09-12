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
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }

        private async Task ProcessBookingRemindersAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var upcomingBookings = await unitOfWork.bookingRepository.GetWhereAndIncludeAsync(
                b => b.CheckInDate.Date == DateTime.Now.AddDays(1).Date,
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
                    await SendReminderAsync(booking);
                    _logger.LogInformation($"Sent reminder for booking ID: {booking.Id}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error sending reminder for booking ID: {booking.Id}");
                }
            }
        }

        private async Task<bool> SendReminderAsync(Booking booking)
        {
            var request = new SendReminderEmail
            {
                UserEmail = booking.User.Email,
                FirstName = booking.User.FirstName,
                checkInDate = booking.CheckInDate,
                BookingId = booking.Id
            };

            return await SendEmailAsync(request);
        }

        private async Task<bool> SendEmailAsync(SendReminderEmail request)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var emailModel = new
            {
                To = request.UserEmail,
                Subject = "Upcoming Booking Reminder",
                Body = $"Hello {request.FirstName}, this is a reminder that your booking is tomorrow, {request.checkInDate.Date}."
            };

            var sendEmail = await httpClient.PostAsJsonAsync("https://localhost:7168/api/EmailService", emailModel);

            if (sendEmail.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Sent reminder email for booking ID: {request.BookingId}");
                return true;
            }

            _logger.LogError($"Error sending reminder email for booking ID: {request.BookingId}");
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
