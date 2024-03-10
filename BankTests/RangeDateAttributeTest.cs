using BankProject;

namespace BankTests;

[TestClass]
public class RangeDateAttributeTest
{

    [TestMethod]
    public void RangeDateAttribute_PassValidDate_ReturnsValid() {
        RangeDateAttribute range = new RangeDateAttribute(-100);
        Assert.IsTrue(range.IsValid(DateTime.UtcNow.AddDays(-1)));
    }

    [TestMethod]
    public void RangeDateAttribute_PassInvalidDate_ReturnsInvalid() {
        RangeDateAttribute range = new RangeDateAttribute(-100);
        Assert.IsFalse(range.IsValid(DateTime.UtcNow.AddDays(1)));
    }

}
