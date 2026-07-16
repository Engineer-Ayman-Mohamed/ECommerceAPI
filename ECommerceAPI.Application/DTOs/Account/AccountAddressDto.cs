namespace ECommerceAPI.Application.DTOs.Account;

public record AccountAddressDto(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country
);
