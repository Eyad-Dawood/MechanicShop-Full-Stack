namespace MechanicShop.Application.Features.Customers.Mappers;


    public static class VehicleMapper
    {
        public static VehicleDto ToDto(this Vehicle entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new VehicleDto(entity.Id, entity.Make!, entity.Model!, entity.Year, entity.LicensePlate!);
        }

        public static List<VehicleDto> ToDtos(this IEnumerable<Vehicle> entities)
        {
            return [.. entities.Select(e => e.ToDto())];
        }
    }
