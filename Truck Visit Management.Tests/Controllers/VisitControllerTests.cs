using Xunit;
using NSubstitute;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Truck_Visit_Management.Controllers;
using Truck_Visit_Management.Dtos;
using Truck_Visit_Management.Services;
using System.Collections.Generic;
using Truck_Visit_Management.Services.ServiceImpl;
using Truck_Visit_Management.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class VisitControllerTests
{
    private readonly IVisitService _visitService;
    private readonly VisitsController _controller;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VisitControllerTests()
    {
        _visitService = Substitute.For<IVisitService>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _controller = new VisitsController(_visitService, _httpContextAccessor);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "testuser")
        }, "mock"));

        _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = user });
    }

  


    [Fact]
    public async Task GetVisits_ReturnsOkResult_WithListOfVisitRecordResponseDto()
    {
        // Arrange
        var visitResponseDtos = new List<VisitRecordResponseDto>
        {
            new VisitRecordResponseDto { Id = 1, Status = "PreRegistered" }
        };
        _visitService.GetVisitsAsync().Returns(visitResponseDtos);

        // Act
        var result = await _controller.GetVisits();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(visitResponseDtos, okResult.Value);
    }

  

    [Fact]
    public async Task GetVisitById_ReturnsOkResult_WithVisitRecordResponseDto()
    {
        // Arrange
        int visitId = 1;
        var visitResponseDto = new VisitRecordResponseDto { Id = visitId, Status = "PreRegistered" };
        _visitService.GetVisitByIdAsync(visitId).Returns(visitResponseDto);

        // Act
        var result = await _controller.GetVisitById(visitId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(visitResponseDto, okResult.Value);
    }

    [Fact]
    public async Task GetVisitById_ReturnsNotFound_WhenVisitDoesNotExist()
    {
        // Arrange
        int visitId = 1;
        _visitService.GetVisitByIdAsync(visitId).Returns((VisitRecordResponseDto)null);

        // Act
        var result = await _controller.GetVisitById(visitId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}
