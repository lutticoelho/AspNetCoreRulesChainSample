using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreRulesChainSample.FakeRepository.Contracts;
using AspNetCoreRulesChainSample.Model;
using AspNetCoreRulesChainSample.Model.RulesContext;
using AspNetCoreRulesChainSample.Rules.ShoppingCartRules;
using RulesChain.Contracts;
using FluentAssertions;
using Moq;
using Xunit;

namespace AspNetCoreRulesChainSample.Rules.UnitTests.ShoppingCartRules
{
    public class IsValidCouponRuleTest
    {
        [Theory(DisplayName = "ShouldRun returns true only in the month of client birthday if CouponCode is in context and in SalesRepository")]
        [InlineData("", false,false)] // No coupon and doesn't exist in repository
        [InlineData(null, false, false)] // No coupon and doesn't exist in repository
        [InlineData("anyCoupon", false, false)] // Has coupon and doesn't exist in repository
        [InlineData("anyCoupon", true, true)] // Has coupon and exist in repository
        public void ShouldRun(string couponValue, bool isCouponAvailable, bool expectedResult)
        {
            // Arrange
            var mockRule = new Mock<IRule<ApplyDiscountContext>>();
            mockRule.Setup(_ => _.ShouldRun(It.IsAny<ApplyDiscountContext>())).Returns(false);

            var mockRepository = new Mock<ISalesRepository>();
            mockRepository.Setup(_ => _.IsCouponAvailable(It.IsAny<string>())).Returns(isCouponAvailable);
            
            var rule = new IsValidCouponRule(mockRule.Object.Invoke, mockRepository.Object);

            var context = new ApplyDiscountContext
            {
                Context = new ShoppingCart
                {
                    CupomCode = couponValue,
                    StartDate = DateTime.Now,
                    Items = new List<ShoppingCartItem>
                    {
                        new ShoppingCartItem{ Id = 1, Name = "Product 1", Price = 1.00m, Quantity = 2},
                        new ShoppingCartItem{ Id = 2, Name = "Product 2", Price = 2.00m, Quantity = 1},
                        new ShoppingCartItem{ Id = 3, Name = "Product 3", Price = 3.50m, Quantity = 1},
                    }
                }
            };

            // Act 
            var shouldRun = rule.ShouldRun(context);

            // Assert
            shouldRun.Should().Be(expectedResult);
        }

        [Fact(DisplayName = "Run should apply discount if no other rule applied a higher discount")]
        public async Task Run_Should_ApplyDiscount_When_NoOtherRuleAppliedAHigherDiscount()
        {
            // Arrange
            var mockRule = new Mock<IRule<ApplyDiscountContext>>();
            mockRule.Setup(_ => _.ShouldRun(It.IsAny<ApplyDiscountContext>())).Returns(false);

            var mockRepository = new Mock<ISalesRepository>();
            
            var rule = new IsValidCouponRule(mockRule.Object.Invoke, mockRepository.Object);
            var context = new ApplyDiscountContext
            {
                ClientBirthday = DateTime.Now,
                Context = new ShoppingCart
                {
                    CupomCode = "cupom-001",
                    StartDate = DateTime.Now,
                    Items = new List<ShoppingCartItem>
                    {
                        new ShoppingCartItem{ Id = 1, Name = "Product 1", Price = 1.00m, Quantity = 5},
                        new ShoppingCartItem{ Id = 2, Name = "Product 2", Price = 2.00m, Quantity = 1},
                        new ShoppingCartItem{ Id = 3, Name = "Product 3", Price = 3.00m, Quantity = 1},
                    }
                }
            };

            // Act 
            await rule.Run(context);

            // Assert
            context.DiscountApplied.Should().Be(0.70m);
            context.DiscountTypeApplied.Should().Be("Coupon");
        }

        [Fact(DisplayName = "Run should not apply discount if other rule applied a higher discount")]
        public async Task Run_ShouldNot_ApplyDiscount_When_OtherRuleAppliedAHigherDiscount()
        {
            // Arrange
            var mockRule = new Mock<IRule<ApplyDiscountContext>>();
            mockRule.Setup(_ => _.ShouldRun(It.IsAny<ApplyDiscountContext>())).Returns(false);

            var mockRepository = new Mock<ISalesRepository>();
            
            var rule = new IsValidCouponRule(mockRule.Object.Invoke, mockRepository.Object);
            var context = new ApplyDiscountContext
            {
                ClientBirthday = DateTime.Now,
                DiscountApplied = 0.8m,
                DiscountTypeApplied = "Other Rule",
                Context = new ShoppingCart
                {
                    CupomCode = "cupom-001",
                    StartDate = DateTime.Now,
                    Items = new List<ShoppingCartItem>
                    {
                        new ShoppingCartItem{ Id = 1, Name = "Product 1", Price = 1.00m, Quantity = 5},
                        new ShoppingCartItem{ Id = 2, Name = "Product 2", Price = 2.00m, Quantity = 1},
                        new ShoppingCartItem{ Id = 3, Name = "Product 3", Price = 3.00m, Quantity = 1},
                    }
                }
            };

            // Act 
            await rule.Run(context);

            // Assert
            context.DiscountApplied.Should().Be(0.80m);
            context.DiscountTypeApplied.Should().Be("Other Rule");
        }
    }
}
