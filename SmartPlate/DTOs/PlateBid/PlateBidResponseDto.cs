using System.ComponentModel.DataAnnotations;
namespace SmartPlate.DTOs.PlateBid
{
    public record class PlateBidResponseDto
    {
        public Guid Id { get; set; }


        //user details
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;


        //plate details
        public Guid PlateListingId { get; set; }
        public Guid PlateId { get; set; }
        public string PlateRegistrationNumber { get; set; } = null!;

        // Suggested price 
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
