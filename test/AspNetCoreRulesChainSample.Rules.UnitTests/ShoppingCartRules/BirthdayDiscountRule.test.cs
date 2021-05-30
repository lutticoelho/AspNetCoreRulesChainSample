using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreRulesChainSample.Model;
using AspNetCoreRulesChainSample.Model.RulesContext;
using AspNetCoreRulesChainSample.Rules.ShoppingCartRules;
using RulesChain.Contracts;
using FluentAssertions;
using Moq;
using Xunit;

namespace AspNetCoreRulesChainSample.Rules.UnitTests.ShoppingCartRules
{
    public class BirthdayDiscountRuleTest
    {
        [Theory(DisplayName = "ShouldRun returns true only in the month of client birthday")]
        [InlineData(60, false)] // After some month after client birthday
        [InlineData(-60, false)] // After some month before client birthday
        [InlineData(0, true)] // The month of client birthday
        public void ShouldRun(int daysFromUtcNow, bool expectedResult)
        {
            // Arrange
            var mockRule = new Mock<IRule<ApplyDiscountContext>>();
            mockRule.Setup(_ => _.ShouldRun(It.IsAny<ApplyDiscountContext>())).Returns(false);

            var rule = new BirthdayDiscountRule(mockRule.Object.Invoke);
            var context = new ApplyDiscountContext
            {
                ClientBirthday = DateTime.Now.AddDays(daysFromUtcNow),
                Context = new ShoppingCart
                {
                    CouponCode = "coupon-001",
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

            var rule = new BirthdayDiscountRule(mockRule.Object.Invoke);
            var context = new ApplyDiscountContext
            {
                ClientBirthday = DateTime.Now,
                Context = new ShoppingCart
                {
                    CouponCode = "coupon-001",
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
            await rule.Run(context);

            // Assert
            context.DiscountApplied.Should().Be(0.65m);
            context.DiscountTypeApplied.Should().Be("Birthday Discount");
        }

        [Fact(DisplayName = "Run should not apply discount if other rule applied a higher discount")]
        public async Task Run_ShouldNot_ApplyDiscount_When_OtherRuleAppliedAHigherDiscount()
        {
            // Arrange
            var mockRule = new Mock<IRule<ApplyDiscountContext>>();
            mockRule.Setup(_ => _.ShouldRun(It.IsAny<ApplyDiscountContext>())).Returns(false);

            var rule = new BirthdayDiscountRule(mockRule.Object.Invoke);
            var context = new ApplyDiscountContext
            {
                ClientBirthday = DateTime.Now,
                DiscountApplied = 0.7m,
                DiscountTypeApplied = "Other Rule",
                Context = new ShoppingCart
                {
                    CouponCode = "coupon-001",
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
            await rule.Run(context);

            // Assert
            context.DiscountApplied.Should().Be(0.70m);
            context.DiscountTypeApplied.Should().Be("Other Rule");
        }
    }
}
