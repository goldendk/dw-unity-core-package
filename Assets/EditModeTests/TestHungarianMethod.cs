using DWGames.com.darkwing_games.core.Runtime.Util;
using NUnit.Framework;

public class TestHungarianMethod
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestHungarianMethodSimplePasses()
    {
        int[,] matrix =
        {
            {82, 83, 69, 92},
            {77, 37, 49, 92},
            {11, 69, 5, 86},
            {8, 9, 98, 23}
        };

        int[] dictionary = new TempHungarian(matrix).Run();

        Assert.AreEqual(2, dictionary[0], 0);
        Assert.AreEqual(1, dictionary[1], 0);
        Assert.AreEqual(0, dictionary[2], 0);
        Assert.AreEqual(3, dictionary[3], 0);
    }
}