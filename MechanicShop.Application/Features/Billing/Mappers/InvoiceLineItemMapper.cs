using MechanicShop.Application.Features.Billing.Dtos;
using MechanicShop.Domain.WorkOrders.Billing.Invoices.InvoiceLineItems;

namespace MechanicShop.Application.Features.Billing.Mappers;

public static class InvoiceLineItemMapper
{
    public static InvoiceLineItemDto ToDto(this InvoiceLineItem item)
    {
        return new InvoiceLineItemDto
        {
            InvoiceId = item.InvoiceId,
            LineNumber = item.LineNumber,
            Description = item.Description,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            LineTotal = item.LineTotal
        };
    }

    public static List<InvoiceLineItemDto> ToDtos(this IEnumerable<InvoiceLineItem> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}
