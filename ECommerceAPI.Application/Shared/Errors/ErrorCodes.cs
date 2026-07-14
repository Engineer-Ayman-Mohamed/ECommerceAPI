namespace ECommerceAPI.Application.Shared.Errors;

public static class ErrorCodes
{
    // General
    public const string NotFound = "NOT_FOUND";
    public const string ValidationError = "VALIDATION_ERROR";
    public const string Unauthorized = "UNAUTHORIZED";
    public const string Forbidden = "FORBIDDEN";
    public const string Conflict = "CONFLICT";

    // Products
    public const string ProductNotFound = "PRODUCT_NOT_FOUND";

    // Brands
    public const string BrandNotFound = "BRAND_NOT_FOUND";
    public const string BrandHasProducts = "BRAND_HAS_PRODUCTS";

    // Types
    public const string TypeNotFound = "TYPE_NOT_FOUND";
    public const string TypeHasProducts = "TYPE_HAS_PRODUCTS";

    // Orders
    public const string OrderNotFound = "ORDER_NOT_FOUND";
    public const string BasketNotFound = "BASKET_NOT_FOUND";
    public const string DeliveryMethodNotFound = "DELIVERY_METHOD_NOT_FOUND";
    public const string EmptyBasket = "EMPTY_BASKET";

    // Basket
    public const string BasketDeleteFailed = "BASKET_DELETE_FAILED";
}
