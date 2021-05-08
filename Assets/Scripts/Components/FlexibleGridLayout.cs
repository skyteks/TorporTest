using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitTypes
        {
            Uniform,
            Width,
            Height,
        }

        public Vector2 spacing;

        public FitTypes fitType;

        public int rows;
        public int columns;
        public Vector2 cellSize;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            float sqrt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrt);
            columns = Mathf.CeilToInt(sqrt);

            if (fitType == FitTypes.Width)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
            }
            if (fitType == FitTypes.Height)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellWidth = (parentWidth / (float)columns) - (spacing.x / (float) columns) - ((padding.left + padding.right) / (float)columns);
            float cellHeight = (parentHeight / (float)rows) - (spacing.y / (float)rows) - ((padding.top + padding.bottom) / (float)rows);

            cellSize.x = cellWidth;
            cellSize.y = cellHeight;

            int columnCount = 0;
            int rowCount = 0;
            for (int i = 0; i < rectChildren.Count; i++)
            {
                rowCount = i / columns;
                columnCount = i % columns;

                var item = rectChildren[i];

                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
        }

        public override void SetLayoutHorizontal()
        {
        }

        public override void SetLayoutVertical()
        {
        }
    }
}
