namespace KafkaConsumer
{
	internal class GradeFreeFormWager
	{
		public GradeFreeFormWager()
		{
		}

		public string AdjustedLossAmount { get; set; }
		public string AdjustedWinAmount { get; set; }
		public string DailyFigureDate_YYYYMMDD { get; set; }
		public bool IsValidTicketNumber { get; set; }
		public string Outcome { get; set; }
		public string TicketNumber { get; set; }
		public string WagerNumber { get; set; }
	}
}