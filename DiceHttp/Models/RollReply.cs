using System.Linq;
namespace DiceHttp.Models
{
    public class RollReply
    {
        public int[] Rolls { get; set; }
        public int Sum => Rolls.Sum();
        public double Mean => Rolls.Average();
    }
}