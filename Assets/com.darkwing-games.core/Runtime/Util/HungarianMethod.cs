using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DWGames.com.darkwing_games.core.Runtime.Util
{
    
    /// <summary>
    ///
    /// https://www.hungarianalgorithm.com/hungarianalgorithm.php
    /// 
    /// <h3>The Hungarian algorithm</h3>
    ///
    /// 
    /// The Hungarian algorithm consists of the four steps below.
    /// The first two steps are executed once, while Steps 3 and 4 are repeated until an optimal assignment is found. The input of the algorithm is an n by n square matrix with only nonnegative elements.
    /// <ol>
    /// <li>
    ///  Step 1: Subtract row minima
    /// For each row, find the lowest element and subtract it from each element in that row.
    /// </li> 
    ///
    /// <li>
    /// Step 2: Subtract column minima
    /// Similarly, for each column, find the lowest element and subtract it from each element in that column.
    /// </li>
    ///
    /// <li>
    /// Step 3: Cover all zeros with a minimum number of lines
    /// Cover all zeros in the resulting matrix using a minimum number of horizontal and vertical lines. If n lines are required, an optimal assignment exists among the zeros. The algorithm stops.
    /// If less than n lines are required, continue with Step 4.
    /// </li>
    ///
    /// <li>
    /// Step 4: Create additional zeros
    /// Find the smallest element (call it k) that is not covered by a line in Step 3. Subtract k from all uncovered elements, and add k to all elements that are covered twice.
    /// </li>
    /// </ol>
    /// </summary>
    public class HungarianMethod
    {
        /// <summary>
        /// Precondition: pre-calculated matrix of the cost of doing the operation for each participant.
        /// <param name="costMatrix">Two-axis matrix of the cost. First axis is partipant and the second is the operation performed. The cell value is the cost of the operation for each participant.
        /// Example: Axis1: index of GameObject, Axis2: distance to a point.</param>
        /// <returns>Dictionary, key is the participant index (GameObject), value is the index of the operation to perform.</returns>
        /// </summary>
        public static Dictionary<int, int> Assign(float[][] costMatrix)
        {
            if (costMatrix.Rank != costMatrix.GetLength(0))
            {
                throw new ArgumentException("Rank and dimension of matrix must be equal");
            }

            var clonedMatrix = costMatrix.Clone();
            
            //step 1
            for (int i = 0; i < costMatrix.GetLength(0); i ++)
            {
                var lowest = float.MaxValue;
                for (var j = 0; j < costMatrix.GetLength(1); j++)
                {
                    var currentCost = costMatrix[i][j]; 
                    if ( currentCost < lowest)
                    {
                        lowest = currentCost;
                    }
                }

                for (var jj = 0; jj < costMatrix.GetLength(1); jj++)
                {
                    costMatrix[i][jj] -= lowest;
                }
            }
            
            //step 2
            for (var col = 0; col < costMatrix.Rank; col++)
            {
                float lowest = float.MaxValue;
                for (var row = 0; row < costMatrix.Rank; row++)
                {
                    var cost = costMatrix[row][col];
                    if (cost < lowest)
                    {
                        lowest = cost;
                    }
                }

                for (var row = 0; row < costMatrix.Rank; row++)
                {
                    costMatrix[row][col] -= lowest;
                }
            }

            var minimumNumberOfLines = FindMinimumNumberOfLinesCoveringZeros(costMatrix);

            return null;

        }

        private static int FindMinimumNumberOfLinesCoveringZeros(float[][] costMatrix)
        {
            var rowCovered = new HashSet<int>();
            var colCovered = new HashSet<int>();
            var rowZeroCount = new Dictionary<int, int>();
            var colZeroCount = new Dictionary<int, int>();
            
            //count numbers in each row and col.
            for (var row = 0; row < costMatrix.Rank; row++)
            {
                for (int col = 0; col < costMatrix.Rank; col++)
                {
                    if (Mathf.Approximately(costMatrix[row][col], 0))
                    {
                        if (!rowZeroCount.ContainsKey(row))
                        {
                            rowZeroCount.Add(row, 0);
                        }

                        if (!colZeroCount.ContainsKey(col))
                        {
                            colZeroCount.Add(col, 0);
                        }
                        
                        rowZeroCount[row] += 1;
                        colZeroCount[col] += 1;
                    }    
                }
            }

            //keys are index, values are the number of zeros.
            var rowSorted = rowZeroCount.OrderBy((e) => e.Value).ToList();
            var colSorted = colZeroCount.OrderBy((e) => e.Value).ToList();
            //int lines = 0;
            while (rowZeroCount.Count > 0 && colZeroCount.Count > 0)
            {
                //if we have both then figure out which to remove from.
                if (rowSorted.Count > 0 && colSorted.Count > 0)
                {
                    if (rowSorted[0].Value >= colSorted[0].Value)
                    {
                        //make line across row.
                        
                    }
                    else
                    {
                     
                        //make line across column
                        //TODO: How do we "crossover" zeros ?
                    }
                }
                //else remove from the one that is not empty.
                
            }
            
            return -1;
        }
    }
}