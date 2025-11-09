namespace SaasFactory.Features.UserSubscriptions.Tests.Integration;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ExampleUnit
{
    [Test]
    public async Task OnePlusTwoEqualsThree()
    {
        const int one = 1;
        const int two = 2;
        const int three = 3;
        
        Assert.That(one + two, Is.EqualTo(three));
    }
}