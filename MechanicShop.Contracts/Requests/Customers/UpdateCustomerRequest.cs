namespace MechanicShop.Contracts.Requests.Customers;

public class UpdateCustomerRequest
{
    public string Name { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public List<UpdateVehicleRequest> Vehicles { get; set; } = [];
}