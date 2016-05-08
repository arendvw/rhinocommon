#pragma warning disable 1591
using System;

namespace Rhino.Render
{
#if RDK_UNCHECKED
  /// <summary>
  /// Content undo helper to be used with "using {}" to enclose a block of changes.
  /// </summary>
  public class ContentUndoHelper : IDisposable
  {
    /// <summary>
    /// Constructs a ContentUndoHelper object inside a using block to handle undo when modifying a RenderContent
    /// or - alternatively - create the ContentUndoHelper and explicitly call Dispose when you are done.
    /// </summary>
    /// <param name="description">Undo description (which appears in the UI when undo is run)</param>
    public ContentUndoHelper(String description)
    {
      m_pUndo = UnsafeNativeMethods.Rdk_ContentUndo_New(description);
    }

    /// <summary>
    /// Call this *after* adding a content. Undo will cause the content to be deleted.
    /// </summary>
    /// <param name="content">Content you just added to the ContentList.</param>
    /// <returns>true if the content was added.</returns>
    public bool AddContent(RenderContent content)
    {
      return 1 == UnsafeNativeMethods.Rdk_ContentUndo_AddContent(ConstPointer(), content.ConstPointer());
    }

    /// <summary>
    ///  Call this before modifying or deleting a content. Undo will cause the content to be restored.
    /// </summary>
    /// <param name="content">Content you are about to modify.</param>
    /// <returns>true if the content was modified.</returns>
    public bool ModifyContent(RenderContent content)
    {
      return 1 == UnsafeNativeMethods.Rdk_ContentUndo_ModifyContent(ConstPointer(), content.ConstPointer());
    }

    /// <summary>
    /// Call this before tweaking a single content parameter. Undo will cause the parameter to be restored.
    /// </summary>
    /// <param name="content">The render content</param>
    /// <param name="parameterName">The parameter name you are about to change.</param>
    /// <returns>true if the content was tweaked.</returns>
    public bool TweakContent(RenderContent content, String parameterName)
    {
      return 1 == UnsafeNativeMethods.Rdk_ContentUndo_TweakContent(ConstPointer(), content.ConstPointer(), parameterName);
    }

    #region IDisposable
    ~ContentUndoHelper()
    {
      Dispose(false);
    }
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool isDisposing)
    {
      UnsafeNativeMethods.Rdk_ContentUndo_Delete(m_pUndo);
    }
    #endregion

    #region internals
    internal IntPtr ConstPointer()
    {
      return m_pUndo;
    }
    private readonly IntPtr m_pUndo;
    #endregion
  }
#endif
#if RDK_CHECKED

  public class ContentUndoBlocker : IDisposable
  {
    /// <summary>
    /// Constructs a ContentUndoBlocker object inside a using block to block undo when modifying a RenderContent
    /// while a ContentUndoHelper is active. Alternatively - create the ContentUndoBlocker and explicitly call Dispose when you are done.
    /// </summary>
    public ContentUndoBlocker()
    {
      m_pUndoBlocker = UnsafeNativeMethods.Rdk_ContentUndo_NewBlocker();
    }

    #region IDisposable
    ~ContentUndoBlocker()
    {
      Dispose(false);
    }
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool isDisposing)
    {
      UnsafeNativeMethods.Rdk_ContentUndo_DeleteBlocker(m_pUndoBlocker);
    }
    #endregion

    #region internals
    internal IntPtr ConstPointer()
    {
      return m_pUndoBlocker;
    }
    private readonly IntPtr m_pUndoBlocker;
    #endregion
  }
#endif
}
