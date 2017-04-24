﻿/*
    Copyright (C) 2014-2017 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.ObjectModel;
using dnSpy.Contracts.Debugger;
using dnSpy.Contracts.Debugger.CallStack;

namespace dnSpy.Debugger.CallStack {
	/// <summary>
	/// Provides all stack frames shown in the call stack window
	/// </summary>
	abstract class CallStackService {
		/// <summary>
		/// Gets the selected thread. This is identical to <see cref="DbgManager.CurrentThread"/>
		/// </summary>
		public abstract DbgThread Thread { get; }

		/// <summary>
		/// Index of active thread. This could be invalid if <see cref="Frames"/> is empty
		/// </summary>
		public abstract int ActiveFrameIndex { get; set; }

		/// <summary>
		/// Raised when <see cref="ActiveFrameIndex"/> is changed
		/// </summary>
		public abstract event EventHandler ActiveFrameIndexChanged;

		/// <summary>
		/// Gets the active frame or null if <see cref="Frames"/> is empty
		/// </summary>
		public DbgStackFrame ActiveFrame {
			get {
				var info = Frames;
				var frames = info.Frames;
				int index = info.ActiveFrameIndex;
				return (uint)index < (uint)frames.Count ? frames[index] : null;
			}
		}

		/// <summary>
		/// Gets all frames. This is a truncated list if there are too many frames. The
		/// frames must not be closed, they're owned by <see cref="CallStackService"/>.
		/// </summary>
		public abstract DbgCallStackFramesInfo Frames { get; }

		/// <summary>
		/// Raised when <see cref="Frames"/> is changed
		/// </summary>
		public abstract event EventHandler FramesChanged;
	}

	/// <summary>
	/// Contains the stack frames and related info
	/// </summary>
	struct DbgCallStackFramesInfo {
		/// <summary>
		/// Gets all frames
		/// </summary>
		public ReadOnlyCollection<DbgStackFrame> Frames { get; }

		/// <summary>
		/// true if there are too many frames and <see cref="Frames"/> is a truncated list of all frames
		/// </summary>
		public bool FramesTruncated { get; }

		/// <summary>
		/// Index of active thread. This could be invalid if <see cref="Frames"/> is empty
		/// </summary>
		public int ActiveFrameIndex { get; }

		/// <summary>
		/// Gets the active frame or null if <see cref="Frames"/> is empty
		/// </summary>
		public DbgStackFrame ActiveStackFrame => (uint)ActiveFrameIndex < (uint)Frames.Count ? Frames[ActiveFrameIndex] : null;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="frames">All frames</param>
		/// <param name="framesTruncated">true if there are too many frames and <paramref name="frames"/> is a truncated list of all frames</param>
		/// <param name="activeFrameIndex">Index of active thread. This could be invalid if <paramref name="frames"/> is empty</param>
		public DbgCallStackFramesInfo(ReadOnlyCollection<DbgStackFrame> frames, bool framesTruncated, int activeFrameIndex) {
			Frames = frames ?? throw new ArgumentNullException(nameof(frames));
			FramesTruncated = framesTruncated;
			ActiveFrameIndex = activeFrameIndex;
		}
	}
}