using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using ContractWhist.Models;
using System.IO;

namespace ContractWhist
{
    public class MLBid
    {
        private const string MODEL_FILEPATH = @"MLModel.zip";
        
        public static int DecideBid(Player player)
        {
            int Bid = 0;

            MLContext mlContext = new MLContext();

            // Training code used by ML.NET CLI and AutoML to generate the model
            //ModelBuilder.CreateModel();

            ITransformer mlModel = mlContext.Model.Load(GetAbsolutePath(MODEL_FILEPATH), out DataViewSchema inputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            // Create sample data to do a single prediction with it 
            ModelInput sampleData = GetModel(player);
            // Try a single prediction
            ModelOutput predictionResult = predEngine.Predict(sampleData);

            Bid = (int)Math.Round(predictionResult.Score);

            return Bid;
        }

        public static ModelInput GetModel(Player player)
        {
            ModelInput modelInput = new ModelInput();
            modelInput.NumberOfTrumpCards = player.NumberOfTrumpCards();
            modelInput.NumberOfAces = player.NumberOfValueCard(14);
            modelInput.NumberOfTwos = player.NumberOfValueCard(2);
            modelInput.TotalValue = player.SumValue();
            return modelInput;
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}
