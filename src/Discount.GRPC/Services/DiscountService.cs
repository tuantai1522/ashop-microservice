using Discount.GRPC.Data;
using Discount.GRPC.Entities;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Services;

public class DiscountService(DiscountContext discountContext) : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly DiscountContext _discountContext = discountContext;
    
    public override async Task<GetDiscountByProductIdResponse> GetDiscountByProductId(GetDiscountByProductIdRequest request, ServerCallContext context)
    {
        var coupons = await _discountContext.Coupons
            .Where(x => x.ProductId == Guid.Parse(request.ProductId))
            .ToListAsync();

        // To built CouponModel from Coupon
        var couponModels = coupons.Adapt<List<CouponModel>>();

        // To build response to return
        var result = new GetDiscountByProductIdResponse();
        result.Coupons.AddRange(couponModels);

        return result;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

        _discountContext.Coupons.Add(coupon);
        await _discountContext.SaveChangesAsync();

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;    
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _discountContext
            .Coupons
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductId={request.Id} is not found."));

        _discountContext.Coupons.Remove(coupon);
        await _discountContext.SaveChangesAsync();

        return new DeleteDiscountResponse { Success = true };

    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

        _discountContext.Coupons.Update(coupon);
        await _discountContext.SaveChangesAsync();

        
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
}