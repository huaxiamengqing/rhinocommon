using System;
using System.Collections.Generic;

namespace Rhino.Geometry
{
  /// <summary>
  /// Represents a hatch in planar boundary loop or loops.
  /// This is a 2d entity with a plane defining a local coordinate system.
  /// The loops, patterns, angles, etc are all in this local coordinate system.
  /// The Hatch object manages the plane and loop array
  /// Fill definitions are in the HatchPattern or class derived from HatchPattern
  /// Hatch has an index to get the pattern definition from the pattern table
  /// </summary>
  public class Hatch : GeometryBase
  {
    internal Hatch(IntPtr native_ptr, Rhino.DocObjects.RhinoObject parent_object, Rhino.DocObjects.ObjRef obj_ref)
      : base(native_ptr, parent_object, obj_ref)
    { }

    internal override GeometryBase DuplicateShallowHelper()
    {
      return new Hatch(IntPtr.Zero, null, null);
    }

    public static Hatch[] Create(IEnumerable<Curve> curves, int hatchPatternIndex, double rotationRadians, double scale)
    {
      Rhino.Runtime.InteropWrappers.SimpleArrayCurvePointer curvearray = new Rhino.Runtime.InteropWrappers.SimpleArrayCurvePointer(curves);
      IntPtr pCurveArray = curvearray.NonConstPointer();
      Rhino.Runtime.InteropWrappers.SimpleArrayGeometryPointer hatcharray = new Rhino.Runtime.InteropWrappers.SimpleArrayGeometryPointer();
      IntPtr pOutput = hatcharray.NonConstPointer();
      UnsafeNativeMethods.RHC_RhinoCreateHatches(pCurveArray, hatchPatternIndex, rotationRadians, scale, pOutput);
      GeometryBase[] g = hatcharray.ToNonConstArray();
      if( g==null )
        return null;
      List<Hatch> hatches = new List<Hatch>();
      for (int i = 0; i < g.Length; i++)
      {
        Hatch hatch = g[i] as Hatch;
        if (hatch != null)
          hatches.Add(hatch);
      }
      return hatches.ToArray();
    }


    public GeometryBase[] Explode()
    {
      Rhino.Runtime.InteropWrappers.SimpleArrayGeometryPointer geometry = new Rhino.Runtime.InteropWrappers.SimpleArrayGeometryPointer();
      IntPtr pParentRhinoObject = IntPtr.Zero;
      if (IsDocumentControlled)
      {
        Rhino.DocObjects.RhinoObject rhobj = ParentRhinoObject();
        if (rhobj != null)
          pParentRhinoObject = rhobj.ConstPointer();
      }
      IntPtr pGeometryArray = geometry.NonConstPointer();
      IntPtr pConstThis = ConstPointer();

      UnsafeNativeMethods.ON_Hatch_Explode(pConstThis, pParentRhinoObject, pGeometryArray);
      GeometryBase[] rc = geometry.ToNonConstArray();
      geometry.Dispose();
      return rc;
    }

    public int PatternIndex
    {
      get
      {
        IntPtr pConstThis = ConstPointer();
        return UnsafeNativeMethods.ON_Hatch_PatternIndex(pConstThis);
      }
      set
      {
        IntPtr pThis = NonConstPointer();
        UnsafeNativeMethods.ON_Hatch_SetPatternIndex(pThis, value);
      }
    }

    public double PatternRotation
    {
      get
      {
        IntPtr pConstThis = ConstPointer();
        return UnsafeNativeMethods.ON_Hatch_GetRotation(pConstThis);
      }
      set
      {
        IntPtr pThis = NonConstPointer();
        UnsafeNativeMethods.ON_Hatch_SetRotation(pThis, value);
      }
    }

    public double PatternScale
    {
      get
      {
        IntPtr pConstThis = ConstPointer();
        return UnsafeNativeMethods.ON_Hatch_GetScale(pConstThis);
      }
      set
      {
        IntPtr pThis = NonConstPointer();
        UnsafeNativeMethods.ON_Hatch_SetScale(pThis, value);
      }
    }
  }
}
