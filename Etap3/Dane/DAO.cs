using System.Collections.Concurrent;
using System.Text.Json;


namespace Data
{
    internal class DAO : IDisposable
    {
        private Task loggingTask;
        private StreamWriter writer;
        private BlockingCollection<BallData> writingQueue;
        private string filePath = "../../../../Dane/log.txt";
        private int Width;
        private int Height;
        public DAO(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            writingQueue = new BlockingCollection<BallData>();
            loggingTask = Task.Run(writeToFile);
        }


        public void addToQueue(IBall ball)
        {
            if (ball == null)
            {
                return;
            }

            BallData ballToSave = new BallData(
                                                ball.Position.X,
                                                ball.Position.Y,
                                                ball.Diameter,
                                                ball.Mass,
                                                ball.Velocity.X,
                                                ball.Velocity.Y,
                                                ball.Id);

            if (!writingQueue.IsAddingCompleted)
            {
                writingQueue.Add(ballToSave);
            }

        }

        private void writeToFile()
        {
            using (writer = new StreamWriter(filePath, append: false))
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;
                writer.Write("[\n");
                writer.Write("{" + string.Format("\n\t\"Width\": {0},\n\t\"Height\": {1}\n", Width, Height) + "}");
                foreach (BallData ball in writingQueue.GetConsumingEnumerable())
                {
                    string log = JsonSerializer.Serialize(ball, options);

                    writer.Write("," + "\n"  + log);

                }
                writer.Write("\n]");
                writer.Flush();
            }
            



        }

        public void Dispose()
        {
            writingQueue.CompleteAdding();      
            loggingTask.Wait();
            loggingTask.Dispose();
        }

        internal class BallData
        {
            public float X { get; set; }
            public float Y { get; set; }
            public int Diameter { get; set; }
            public int Mass { get; set; }
            public string Time { get; set; }
            public float VelocityX { get; set; }
            public float VelocityY { get; set; }
            public int Id { get; set; }


            public BallData(float x, float y, int diameter, int mass, float velX, float velY, int id)
            {
                X = x;
                Y = y;
                Diameter = diameter;
                Mass = mass;
                Time = DateTime.UtcNow.ToString("G");
                VelocityX = velX;
                VelocityY = velY;
                Id = id;

            }
        }
    }
}
