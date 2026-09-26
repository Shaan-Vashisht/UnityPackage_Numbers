using System;
using UnityEngine;

namespace SV.Numbers
{
    [Serializable]
    public class Matrix<T>
    {
        [SerializeField]
        private int rows;
        public int Rows => rows;
        
        [SerializeField]
        private int columns;
        public int Columns => columns;
        
        [SerializeField] 
        private T[] values;

        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            values = new T[rows * columns];
        }

        /// <summary>
        /// Access an element of the matrix.
        /// </summary>
        /// <param name="x">The horizontal coordinate of the element (column).</param>
        /// <param name="y">The vertical coordinate of the element (row).</param>
        /// <exception cref="IndexOutOfRangeException">If the coordinates are beyond the range of the matrix.</exception>
        public T this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= columns || y < 0 || y >= rows)
                    throw new IndexOutOfRangeException($"Indexes {x},{y} out of range {columns},{rows}");
                    return values[x + y * columns];
            }
            set
            {
                if (x < 0 || x >= columns || y < 0 || y >= rows)
                    throw new IndexOutOfRangeException($"Indexes {x},{y} out of range {columns},{rows}");
                values[x + y * columns] = value;
            }
        }

        public T[,] To2DArray()
        {
            T[,] array = new T[columns, rows];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    array[x, y] = this[x, y];
                }
            }
            return array;
        }

        public override string ToString()
        {
            string res = "";

            for (int r = 0; r < rows; r++)
            {
                res += "|";
                for (int c = 0; c < columns; c++)
                    res += $" {this[c, r]} ";
                res += "|\n";
            }
            return res;
        }
    }
}