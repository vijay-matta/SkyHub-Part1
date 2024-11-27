using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyHub.Migrations
{
    /// <inheritdoc />
    public partial class firstdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Roles");

            migrationBuilder.EnsureSchema(
                name: "Flight_Details");

            migrationBuilder.EnsureSchema(
                name: "Payment_Details");

            migrationBuilder.CreateTable(
                name: "SeatTypes",
                schema: "Flight_Details",
                columns: table => new
                {
                    SeatTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeatTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BaseFare = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatTypes", x => x.SeatTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Roles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                schema: "Roles",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.AdminId);
                    table.ForeignKey(
                        name: "FK_Admin_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Roles",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightOwner",
                schema: "Roles",
                columns: table => new
                {
                    FlightOwnerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightOwner", x => x.FlightOwnerId);
                    table.ForeignKey(
                        name: "FK_FlightOwner_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Roles",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passenger",
                schema: "Roles",
                columns: table => new
                {
                    PassengerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passenger", x => x.PassengerId);
                    table.ForeignKey(
                        name: "FK_Passenger_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Roles",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                schema: "Flight_Details",
                columns: table => new
                {
                    RouteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Origin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Distance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlightOwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.RouteId);
                    table.ForeignKey(
                        name: "FK_Routes_FlightOwner_FlightOwnerId",
                        column: x => x.FlightOwnerId,
                        principalSchema: "Roles",
                        principalTable: "FlightOwner",
                        principalColumn: "FlightOwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                schema: "Flight_Details",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FlightName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartureTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "date", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "date", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "date", nullable: true),
                    ReturnTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    Fare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    FlightOwnerId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    IsRoundTrip = table.Column<bool>(type: "bit", nullable: false),
                    ReturnFlightId = table.Column<int>(type: "int", nullable: true),
                    FlightOwnerId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightId);
                    table.ForeignKey(
                        name: "FK_Flights_FlightOwner_FlightOwnerId",
                        column: x => x.FlightOwnerId,
                        principalSchema: "Roles",
                        principalTable: "FlightOwner",
                        principalColumn: "FlightOwnerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flights_FlightOwner_FlightOwnerId1",
                        column: x => x.FlightOwnerId1,
                        principalSchema: "Roles",
                        principalTable: "FlightOwner",
                        principalColumn: "FlightOwnerId");
                    table.ForeignKey(
                        name: "FK_Flights_Flights_ReturnFlightId",
                        column: x => x.ReturnFlightId,
                        principalSchema: "Flight_Details",
                        principalTable: "Flights",
                        principalColumn: "FlightId");
                    table.ForeignKey(
                        name: "FK_Flights_Routes_RouteId",
                        column: x => x.RouteId,
                        principalSchema: "Flight_Details",
                        principalTable: "Routes",
                        principalColumn: "RouteId");
                });

            migrationBuilder.CreateTable(
                name: "BaggageInfo",
                schema: "Flight_Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckinWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CabinWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExcessBaggageRate = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaggageInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaggageInfo_Flights_FlightId",
                        column: x => x.FlightId,
                        principalSchema: "Flight_Details",
                        principalTable: "Flights",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                schema: "Flight_Details",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumSeats = table.Column<int>(type: "int", nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Flights_FlightId",
                        column: x => x.FlightId,
                        principalSchema: "Flight_Details",
                        principalTable: "Flights",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Roles",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                schema: "Flight_Details",
                columns: table => new
                {
                    SeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SeatTypeId = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.SeatId);
                    table.ForeignKey(
                        name: "FK_Seats_Flights_FlightId",
                        column: x => x.FlightId,
                        principalSchema: "Flight_Details",
                        principalTable: "Flights",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seats_SeatTypes_SeatTypeId",
                        column: x => x.SeatTypeId,
                        principalSchema: "Flight_Details",
                        principalTable: "SeatTypes",
                        principalColumn: "SeatTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "Payment_Details",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalSchema: "Flight_Details",
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingItems",
                schema: "Flight_Details",
                columns: table => new
                {
                    BookingItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false),
                    SeatTypeId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingItems", x => x.BookingItemId);
                    table.ForeignKey(
                        name: "FK_BookingItems_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalSchema: "Flight_Details",
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookingItems_SeatTypes_SeatTypeId",
                        column: x => x.SeatTypeId,
                        principalSchema: "Flight_Details",
                        principalTable: "SeatTypes",
                        principalColumn: "SeatTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingItems_Seats_SeatId",
                        column: x => x.SeatId,
                        principalSchema: "Flight_Details",
                        principalTable: "Seats",
                        principalColumn: "SeatId");
                });

            migrationBuilder.CreateTable(
                name: "Refunds",
                schema: "Payment_Details",
                columns: table => new
                {
                    RefundId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RefundMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RefundDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refunds", x => x.RefundId);
                    table.ForeignKey(
                        name: "FK_Refunds_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "Payment_Details",
                        principalTable: "Payments",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admin_UserId",
                schema: "Roles",
                table: "Admin",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaggageInfo_FlightId",
                schema: "Flight_Details",
                table: "BaggageInfo",
                column: "FlightId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingItems_BookingId",
                schema: "Flight_Details",
                table: "BookingItems",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingItems_SeatId",
                schema: "Flight_Details",
                table: "BookingItems",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingItems_SeatTypeId",
                schema: "Flight_Details",
                table: "BookingItems",
                column: "SeatTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FlightId",
                schema: "Flight_Details",
                table: "Bookings",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                schema: "Flight_Details",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightOwner_CompanyName",
                schema: "Roles",
                table: "FlightOwner",
                column: "CompanyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightOwner_UserId",
                schema: "Roles",
                table: "FlightOwner",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightNumber",
                schema: "Flight_Details",
                table: "Flights",
                column: "FlightNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightOwnerId",
                schema: "Flight_Details",
                table: "Flights",
                column: "FlightOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightOwnerId1",
                schema: "Flight_Details",
                table: "Flights",
                column: "FlightOwnerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ReturnFlightId",
                schema: "Flight_Details",
                table: "Flights",
                column: "ReturnFlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_RouteId",
                schema: "Flight_Details",
                table: "Flights",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Passenger_UserId",
                schema: "Roles",
                table: "Passenger",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                schema: "Payment_Details",
                table: "Payments",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionId",
                schema: "Payment_Details",
                table: "Payments",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_PaymentId",
                schema: "Payment_Details",
                table: "Refunds",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_FlightOwnerId",
                schema: "Flight_Details",
                table: "Routes",
                column: "FlightOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_FlightId",
                schema: "Flight_Details",
                table: "Seats",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SeatNumber",
                schema: "Flight_Details",
                table: "Seats",
                column: "SeatNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SeatTypeId",
                schema: "Flight_Details",
                table: "Seats",
                column: "SeatTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "Roles",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin",
                schema: "Roles");

            migrationBuilder.DropTable(
                name: "BaggageInfo",
                schema: "Flight_Details");

            migrationBuilder.DropTable(
                name: "BookingItems",
                schema: "Flight_Details");

            migrationBuilder.DropTable(
                name: "Passenger",
                schema: "Roles");

            migrationBuilder.DropTable(
                name: "Refunds",
                schema: "Payment_Details");

            migrationBuilder.DropTable(
                name: "Seats",
                schema: "Flight_Details");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "Payment_Details");

            migrationBuilder.DropTable(
                name: "SeatTypes",
                schema: "Flight_Details");

            migrationBuilder.DropTable(
                name: "Bookings",
                schema: "Flight_Details");

            migrationBuilder.DropTable(
                name: "Flights",
                schema: "Flight_Details");

            migrationBuilder.DropTable(
                name: "Routes",
                schema: "Flight_Details");

            migrationBuilder.DropTable(
                name: "FlightOwner",
                schema: "Roles");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Roles");
        }
    }
}
