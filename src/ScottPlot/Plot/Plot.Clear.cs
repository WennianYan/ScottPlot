﻿/* This file contains API methods to remote plottables from the list */
using System;
using System.Collections.Generic;
using System.Text;
using ScottPlot.Plottable;

namespace ScottPlot
{
    public partial class Plot
    {
        /// <summary>
        /// Remove the given plottable from the plot
        /// </summary>
        public void Remove(IPlottable plottable)
        {
            settings.plottables.Remove(plottable);
        }

        /// <summary>
        /// Clear all plottables
        /// </summary>
        public void Clear()
        {
            settings.plottables.Clear();
            settings.axes.Reset();
        }

        /// <summary>
        /// Clear all plottables matching the given type
        /// </summary>
        public void Clear<T>()
        {
            settings.plottables.RemoveAll(x => x is T);

            if (settings.plottables.Count == 0)
                settings.axes.Reset();
        }

        /// <summary>
        /// Clear all plottables matching the given type
        /// </summary>
        public void Clear(Type typeToClear)
        {
            settings.plottables.RemoveAll(x => x.GetType() == typeToClear);

            if (settings.plottables.Count == 0)
                settings.axes.Reset();
        }

        /// <summary>
        /// Clear all plottables of the same type as the one that is given
        /// </summary>
        public void Clear(IPlottable examplePlottable)
        {
            settings.plottables.RemoveAll(x => x.GetType() == examplePlottable.GetType());

            if (settings.plottables.Count == 0)
                settings.axes.Reset();
        }

        /// <summary>
        /// Clear all plottables matching the given types
        /// </summary>
        public void Clear(Type[] typesToClear)
        {
            if (typesToClear != null)
                foreach (var typeToClear in typesToClear)
                    Clear(typeToClear);
        }

        /// <summary>
        /// Remove the given plottables from the plot
        /// </summary>
        public void Clear(Predicate<IPlottable> plottablesToClear)
        {
            if (plottablesToClear != null)
                settings.plottables.RemoveAll(plottablesToClear);

            if (settings.plottables.Count == 0)
                settings.axes.Reset();
        }

        [Obsolete("This overload is deprecated. Clear plots using a different overload of the Clear() method.")]
        public void Clear(
            bool axisLines = true,
            bool scatterPlots = true,
            bool signalPlots = true,
            bool text = true,
            bool bar = true,
            bool finance = true,
            bool axisSpans = true
            )
        {
            List<int> indicesToDelete = new List<int>();
            for (int i = 0; i < settings.plottables.Count; i++)
            {
                if ((settings.plottables[i] is VLine || settings.plottables[i] is HLine) && axisLines)
                    indicesToDelete.Add(i);
                else if (settings.plottables[i] is PlottableScatter && scatterPlots)
                    indicesToDelete.Add(i);
                else if (settings.plottables[i] is PlottableSignal && signalPlots)
                    indicesToDelete.Add(i);
                else if (settings.plottables[i].GetType().IsGenericType && settings.plottables[i].GetType().GetGenericTypeDefinition() == typeof(PlottableSignalConst<>) && signalPlots)
                    indicesToDelete.Add(i);
                else if (settings.plottables[i] is PlottableText && text)
                    indicesToDelete.Add(i);
                else if (settings.plottables[i] is PlottableBar && bar)
                    indicesToDelete.Add(i);
                else if (settings.plottables[i] is PlottableOHLC && finance)
                    indicesToDelete.Add(i);
                else if ((settings.plottables[i] is VSpan || settings.plottables[i] is HSpan) && axisSpans)
                    indicesToDelete.Add(i);
            }

            indicesToDelete.Reverse();
            for (int i = 0; i < indicesToDelete.Count; i++)
                settings.plottables.RemoveAt(indicesToDelete[i]);

            settings.axes.Reset();
        }
    }
}
