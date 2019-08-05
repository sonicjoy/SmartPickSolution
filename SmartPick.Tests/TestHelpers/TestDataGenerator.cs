using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SmartPick.Core.Models;

namespace SmartPick.Tests.TestHelpers
{
    public class TestDataGenerator
    {
        public static LegModel[] GenerateLegs(int legCount, int binPerLeg)
        {
            var rand = new Random();

            var legs = new LegModel[legCount];
            for (var legOrder = 1; legOrder <= legCount; legOrder++)
            {
                var selections= new SelectionModel[binPerLeg];
                for (var bin = 1; bin <= binPerLeg; bin++)
                {
                    var prob = rand.NextDouble();
                    selections[bin - 1] = new SelectionModel(bin, prob);
                }
                //normalised the probs
                var sumProbs = selections.Sum(s => s.Probability);
                foreach (var sel in selections)
                {
                    var newProb = sel.Probability / sumProbs;
                    sel.UpdateProbability(newProb);
                }

                var sumOfProbs = selections.Sum(s => s.Probability);
                Assert.That(sumOfProbs > 0.99);
                legs[legOrder - 1] = new LegModel(legOrder, selections);
            }

            return legs;
        }

        public static LegModel[] FixedPlace6LegModels()
        {
            var legs = new[]
            {
                new LegModel(1, new[]
                {
                    new SelectionModel(1, 0d),
                    new SelectionModel(2, 0.41012884d),
                    new SelectionModel(3, 0.54686704d),
                    new SelectionModel(4, 0.4135188d),
                    new SelectionModel(5, 0.07554743d),
                    new SelectionModel(6, 0.12355552d),
                    new SelectionModel(7, 0.61202232d),
                    new SelectionModel(8, 0.39090165d),
                    new SelectionModel(9, 0.26337379d),
                    new SelectionModel(10, 0.16408461d),
                    new SelectionModel(11, 0d),
                }),
                new LegModel(2, new[]
                {
                    new SelectionModel(1, 0.04207584d),
                    new SelectionModel(2, 0d),
                    new SelectionModel(3, 0.22451534d),
                    new SelectionModel(4, 0.25136215d),
                    new SelectionModel(5, 0.10869591d),
                    new SelectionModel(6, 0.57711544d),
                    new SelectionModel(7, 0.40094956d),
                    new SelectionModel(8, 0.10786715d),
                    new SelectionModel(9, 0.07363272d),
                    new SelectionModel(10, 0.09639192d),
                    new SelectionModel(11, 0.04145403d),
                    new SelectionModel(12, 0.25261291d),
                    new SelectionModel(13, 0.63711682d),
                    new SelectionModel(14, 0.12344253d),
                    new SelectionModel(15, 0.04568234d),
                    new SelectionModel(16, 0.01708534d),

                }),
                new LegModel(3, new[]
                {
                    new SelectionModel(1, 0.261684d),
                    new SelectionModel(2, 0.66248556d),
                    new SelectionModel(3, 0.78975928d),
                    new SelectionModel(4, 0.04585644d),
                    new SelectionModel(5, 0.0386671d),
                    new SelectionModel(6, 0.46187544d),
                    new SelectionModel(7, 0.56569148d),
                    new SelectionModel(8, 0.10169734d),
                    new SelectionModel(9, 0.07228334d),
                    new SelectionModel(10, 0d),
                }),
                new LegModel(4, new[]
                {
                    new SelectionModel(1, 0.19547455d),
                    new SelectionModel(2, 0.33749908d),
                    new SelectionModel(3, 0.52083679d),
                    new SelectionModel(4, 0d),
                    new SelectionModel(5, 0.20982845d),
                    new SelectionModel(6, 0.21427018d),
                    new SelectionModel(7, 0.29093972d),
                    new SelectionModel(8, 0.23115122d),
                }),
                new LegModel(5, new[]
                {
                    new SelectionModel(1, 0.39761121d),
                    new SelectionModel(2, 0.13626444d),
                    new SelectionModel(3, 0.42285816d),
                    new SelectionModel(4, 0.74478932d),
                    new SelectionModel(5, 0.44723556d),
                    new SelectionModel(6, 0.28223283d),
                    new SelectionModel(7, 0.08263411d),
                    new SelectionModel(8, 0.22271616d),
                    new SelectionModel(9, 0.26365822d),
                }),
                new LegModel(6, new[]
                {
                    new SelectionModel(1, 0.25767754d),
                    new SelectionModel(2, 0.28161457d),
                    new SelectionModel(3, 0d),
                    new SelectionModel(4, 0.09430275d),
                    new SelectionModel(5, 0.36640514d),
                }),
            };
            
            return legs;
        }

        public static LegModel[] FixedWin6LegModels()
        {
            var legs = new[]
            {
                new LegModel(1, new[]
                {
                    new SelectionModel(1, 0.17964072d),
                    new SelectionModel(2, 0.071856287d),
                    new SelectionModel(3, 0.035928143d),
                    new SelectionModel(4, 0.143712577d),
                    new SelectionModel(5, 0.08982036d),
                    new SelectionModel(6, 0.11976048d),
                    new SelectionModel(7, 0.11976048d),
                    new SelectionModel(8, 0.11976048d),
                    new SelectionModel(9, 0.11976048d),
                }),
                new LegModel(2, new[]
                {
                    new SelectionModel(1, 0.28125d),
                    new SelectionModel(2, 0.21875d),
                    new SelectionModel(3, 0.125d),
                    new SelectionModel(4, 0.3125d),
                    new SelectionModel(5, 0.0625d),
                }),
                new LegModel(3, new[]
                {
                    new SelectionModel(1, 0.103448277d),
                    new SelectionModel(2, 0.078369907d),
                    new SelectionModel(3, 0.156739813d),
                    new SelectionModel(4, 0.12539185d),
                    new SelectionModel(5, 0.112852663d),
                    new SelectionModel(6, 0.047021943d),
                    new SelectionModel(7, 0.062695923d),
                    new SelectionModel(8, 0.031347963d),
                    new SelectionModel(9, 0.188087773d),
                    new SelectionModel(10, 0.094043887d),
                }),
                new LegModel(4, new[]
                {
                    new SelectionModel(1, 0.318817307d),
                    new SelectionModel(2, 0.177120727d),
                    new SelectionModel(3, 0.118080483d),
                    new SelectionModel(4, 0.041968427d),
                    new SelectionModel(5, 0.04959905d),
                    new SelectionModel(6, 0.04408338d),
                    new SelectionModel(7, 0.08816676d),
                    new SelectionModel(8, 0.118080483d),
                    new SelectionModel(9, 0.04408338d),
                }),
                new LegModel(5, new[]
                {
                    new SelectionModel(1, 0.306748467d),
                    new SelectionModel(2, 0.122699387),
                    new SelectionModel(3, 0.09202454),
                    new SelectionModel(4, 0.101226993),
                    new SelectionModel(5, 0.0398773),
                    new SelectionModel(6, 0.061349693),
                    new SelectionModel(7, 0.030674847),
                    new SelectionModel(8, 0.122699387),
                    new SelectionModel(9, 0.030674847),
                    new SelectionModel(10, 0.09202454),
                }),
                new LegModel(6, new[]
                {
                    new SelectionModel(1, 0.18348624),
                    new SelectionModel(2, 0.18348624),
                    new SelectionModel(3, 0.071100917),
                    new SelectionModel(4, 0.073394497),
                    new SelectionModel(5, 0.075688073),
                    new SelectionModel(6, 0.07798165),
                    new SelectionModel(7, 0.08027523),
                    new SelectionModel(8, 0.082568807),
                    new SelectionModel(9, 0.084862387),                    
                    new SelectionModel(10, 0.087155963),
                }),
            };
            
            return legs;
        }
    }
}
