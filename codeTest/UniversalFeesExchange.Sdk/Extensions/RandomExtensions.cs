namespace UniversalFeesExchange.Sdk.Extensions
{
    public static class RandomExtensions
    {
        /// <summary>
        /// 10 to the 28th power.
        /// </summary>
        private const byte Scale = 28;

        public static decimal NextDecimal(this Random rnd, int from, int to)
        {
            decimal dec = new decimal(rnd.Next(), rnd.Next(), rnd.Next(), false, Scale);

            if (Math.Sign(from) == Math.Sign(to) || from == 0 || to == 0)
                return decimal.Remainder(dec, to - from) + from;

            return decimal.Remainder(dec, to);
        }
    }
}
