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
using System.Threading.Tasks;
using Foundation;
using MonoDevelop.Debugger.VisualComponents;

namespace MonoDevelop.Debugger.UIApp
{
	public sealed class MacAppCommonServices : CommonServices
	{
		private NSObject taskObject = new NSObject ();

		public override Task RunInMainThread (Action action)
		{
			var tcs = new TaskCompletionSource<bool> ();

			taskObject.InvokeOnMainThread (() => {
				try {
					action ();
					tcs.TrySetResult (true);
				} finally
				{
					tcs.TrySetResult (false);
				}
			});

			return tcs.Task;
		}

		public override void LogInfo (string message, params string [] args)
		{
			Console.WriteLine ($"INFO: {string.Format(message, args)}");
		}

		public override void LogError (string message, Exception ex)
		{
			Console.WriteLine ($"ERROR: {message}: {ex}");
		}

		public override void LogInternalError (Exception ex)
		{
			Console.WriteLine ($"INTERNAL ERROR: {ex}");
		}

		public override IPinnedWatch CreatePinnedWatch ()
		{
			throw new NotImplementedException ();
		}
	}
}
