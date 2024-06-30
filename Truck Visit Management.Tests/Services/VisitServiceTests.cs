
using AutoMapper;
using NSubstitute;
using Truck_Visit_Management.Dtos;
using Truck_Visit_Management.Entities;
using Truck_Visit_Management.Enums;
using Truck_Visit_Management.Exceptions;
using Truck_Visit_Management.Repositories;
using Truck_Visit_Management.Services;
using Truck_Visit_Management.Services.ServiceImpl;


namespace Truck_Visit_Management.Tests.Services
{
    public class VisitServiceTests
    {
        private readonly IVisitService _visitService;
        private readonly IVisitRepository _visitRepository = Substitute.For<IVisitRepository>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();

        public VisitServiceTests()
        {
            _visitService = new VisitService(_visitRepository, _mapper);
        }

        [Fact]
        public async Task CreateVisitAsync_ShouldReturnVisitRecordResponseDto()
        {
            // Arrange
            var visitRequestDto = new VisitRecordRequestDto
            {
                Status = "Pre-Registered",
                TruckLicensePlate = "ABC123",
                Driver = new DriverInformationDto(),
                CreatedBy = "admin"
            };

            var visitEntity = new VisitRecordEntity
            {
                Id = 1,
                Status = "Pre-Registered",
                TruckLicensePlate = "ABC123",
                Driver = new DriverInformationEntity(),
                CreatedBy = "admin"
            };

            var visitResponseDto = new VisitRecordResponseDto
            {
                Id = 1,
                Status = "PreRegistered",
                TruckLicensePlate = "ABC123",
                Driver = new DriverInformationDto(),
                CreatedBy = "admin"
            };

            _mapper.Map<VisitRecordEntity>(visitRequestDto).Returns(visitEntity);
            _visitRepository.CreateVisitAsync(visitEntity).Returns(visitEntity);
            _mapper.Map<VisitRecordResponseDto>(visitEntity).Returns(visitResponseDto);

            // Act
            var result = await _visitService.CreateVisitAsync(visitRequestDto);

            // Assert
            Assert.Equal(visitResponseDto, result);
        }

        [Fact]
        public async Task GetVisitsAsync_ShouldReturnListOfVisitRecordResponseDto()
        {
            // Arrange
            var visitEntities = new List<VisitRecordEntity>
            {
                new VisitRecordEntity
                {
                    Id = 1,
                    Status = "Pre-Registered",
                    TruckLicensePlate = "ABC123",
                    Driver = new DriverInformationEntity()
                }
            };

            var visitResponseDtos = new List<VisitRecordResponseDto>
            {
                new VisitRecordResponseDto
                {
                    Id = 1,
                    Status = "PreRegistered",
                    TruckLicensePlate = "ABC123",
                    Driver = new DriverInformationDto()
                }
            };

            _visitRepository.GetVisitsAsync().Returns(visitEntities);
            _mapper.Map<IEnumerable<VisitRecordResponseDto>>(visitEntities).Returns(visitResponseDtos);

            // Act
            var result = await _visitService.GetVisitsAsync();

            // Assert
            Assert.Equal(visitResponseDtos, result);
        }

        [Fact]
        public async Task UpdateVisitStatusAsync_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            int visitId = 1;
            string status = "Completed";
            string updatedBy = "admin";

            var visitEntity = new VisitRecordEntity
            {
                Id = visitId,
                Status = "Pre-Registered",
                TruckLicensePlate = "ABC123",
                Driver = new DriverInformationEntity(),
                UpdatedBy = "admin"
            };

            _visitRepository.GetVisitByIdAsync(visitId).Returns(visitEntity);

            // Act
            await _visitService.UpdateVisitStatusAsync(visitId, status, updatedBy);

            // Assert
            await _visitRepository.Received(1).UpdateVisitStatusAsync(visitEntity);
        }

        [Fact]
        public async Task GetVisitByIdAsync_ShouldReturnVisitRecordResponseDto()
        {
            // Arrange
            int visitId = 1;
            var visitEntity = new VisitRecordEntity
            {
                Id = visitId,
                Status = "Pre-Registered",
                TruckLicensePlate = "ABC123",
                Driver = new DriverInformationEntity()
            };

            var visitResponseDto = new VisitRecordResponseDto
            {
                Id = visitId,
                Status = "PreRegistered",
                TruckLicensePlate = "ABC123",
                Driver = new DriverInformationDto()
            };

            _visitRepository.GetVisitByIdAsync(visitId).Returns(visitEntity);
            _mapper.Map<VisitRecordResponseDto>(visitEntity).Returns(visitResponseDto);

            // Act
            var result = await _visitService.GetVisitByIdAsync(visitId);

            // Assert
            Assert.Equal(visitResponseDto, result);
        }

        [Fact]
        public async Task CreateVisitAsync_ShouldThrowBadRequestException_WhenValidationFails()
        {
            // Arrange
            var visitRequestDto = new VisitRecordRequestDto
            {
                // Invalid data to trigger validation errors
                TruckLicensePlate = "",
                Driver = new DriverInformationDto(),
                CreatedBy = "admin"
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _visitService.CreateVisitAsync(visitRequestDto));
        }
    }
}
