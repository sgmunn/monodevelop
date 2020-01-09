//
// EmptyClass.cs
//
// Author:
//       Greg Munn <gregm@microsoft.com>
//
// Copyright (c) 2020 (c) Microsoft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MonoDevelop.Debugger.VisualComponents
{
	public abstract class CommonServices : IPinnedWatchFactory
	{
		public static CommonServices Instance { get; set; }

		public abstract Task RunInMainThread (Action action);

		public abstract void LogInfo (string message, params string[] args);

		public abstract void LogError (string message, Exception ex);

		public abstract void LogInternalError (Exception ex);

		public abstract IPinnedWatch CreatePinnedWatch ();

	}

	public class CommonStrings
	{
		public static CommonStrings Localized { get; set; }


	}

	public static class CommonExtensions
	{
		/// <summary>
		/// Use this method to explicitly indicate that you don't care
		/// about the result of an async call
		/// </summary>
		/// <param name="task">The task to forget</param>
		public static void Ignore (this Task task, [CallerMemberName]string operationName = null)
		{
			_ = task.ContinueWith (t => {
				CommonServices.Instance.LogError ($"Async operation `{operationName}` failed", t.Exception);
			}, default, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Current);
		}


		public static int IndexOf<T> (this IEnumerable<T> e, T item)
		{
			bool found = false;
			int index = e.TakeWhile (i => {
				found = EqualityComparer<T>.Default.Equals (i, item);
				return !found;
			}).Count ();

			return found ? index : -1;
		}

	}
}