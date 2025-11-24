using System;
using System.Collections.Generic;
using SmartPlate.Models.Enums;

namespace SmartPlate.Models
{
    public class Plate
    {
        // Parameterless constructor for EF Core
        private Plate()
        {
            Categories = new List<PlateCategory>();
        }

        // Private constructor to initialise Plate with required properties
        private Plate(string registrationNumber, PlateType type, string region, int? yearIssued,
                      bool canApplyToAnyVehicle, bool isAssigned, bool availableAsCertificate,
                      PlateSupplyType supply, IEnumerable<PlateCategory> categories)
        {
            Id = Guid.NewGuid();
            RegistrationNumber = registrationNumber;
            Type = type;
            Region = region;
            YearIssued = yearIssued;
            CanApplyToAnyVehicle = canApplyToAnyVehicle;
            IsAssigned = isAssigned;
            AvailableAsCertificate = availableAsCertificate;
            Supply = supply;

            // Categories are fixed and assigned at creation
            Categories = new List<PlateCategory>(categories);
        }

        public Guid Id { get; private set; }

        // Core identity
        public string RegistrationNumber { get; private set; } = null!;
        public PlateType Type { get; private set; }
        // Fixed categories assigned at creation
        public IReadOnlyCollection<PlateCategory> Categories { get; private set; }

        // Classification
        public string Region { get; private set; } = null!;
        public int? YearIssued { get; private set; }

        // Assignment / DVLA rules
        public bool CanApplyToAnyVehicle { get; private set; }
        public bool IsAssigned { get; private set; }
        public bool AvailableAsCertificate { get; private set; }

        public PlateSupplyType Supply { get; private set; }

        // Factory method to create a new Plate instance
        public static Plate Create(
            string registrationNumber,
            PlateType type,
            string region,
            int? yearIssued,
            bool canApplyToAnyVehicle,
            bool isAssigned,
            bool availableAsCertificate,
            PlateSupplyType supply,
            IEnumerable<PlateCategory> categories)
        {
            if (categories == null)
                throw new ArgumentNullException(nameof(categories), "Plate must have at least one category.");

            return new Plate(
                registrationNumber,
                type,
                region,
                yearIssued,
                canApplyToAnyVehicle,
                isAssigned,
                availableAsCertificate,
                supply,
                categories
            );
        }

        //updating fields
        public void UpdateRegistrationNumber(string registrationNumber)
        {
            if (string.IsNullOrWhiteSpace(registrationNumber))
                throw new ArgumentException("Registration number cannot be empty.");
            RegistrationNumber = registrationNumber;
        }

        public void UpdateRegion(string region)
        {
            if (string.IsNullOrWhiteSpace(region))
                throw new ArgumentException("Region cannot be empty.");
            Region = region;
        }

        public void UpdateCategories(List<PlateCategory> categories)
        {
            if (categories == null || !categories.Any())
                throw new ArgumentException("Plate must have at least one category.");
            Categories = new List<PlateCategory>(categories);
        }

        public void UpdateType(PlateType type)
        {
            Type = type;
        }

        public void UpdateYearIssued(int? year)
        {
            if (year.HasValue && (year < 1900 || year > 2025))
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be between 1900 and 2025.");
            YearIssued = year;
        }

        public void UpdateCanApplyToAnyVehicle(bool canApply)
        {
            CanApplyToAnyVehicle = canApply;
        }

        public void UpdateIsAssigned(bool isAssigned)
        {
            IsAssigned = isAssigned;
        }

        public void UpdateAvailableAsCertificate(bool available)
        {
            AvailableAsCertificate = available;
        }

        public void UpdateSupply(PlateSupplyType supply)
        {
            Supply = supply;
        }
    }
}