using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BlendShapeCopyTool {
    public class BlendShapeCopyMenu : MonoBehaviour {
        [SerializeField]
        static List<(string, float)> _copiedBlendShapes = new List<(string, float)>();

        [MenuItem("CONTEXT/SkinnedMeshRenderer/Copy BlendShapes", false, 120)]
        static private void CopyBlendShape(MenuCommand cmd) {
            SkinnedMeshRenderer smrCmp = cmd.context as SkinnedMeshRenderer;
            List<(string, float)> blendshapes = ExtractBlendShapes(smrCmp);

            _copiedBlendShapes = blendshapes;

            // Debug.Log("Copy BlendShapes: success");
        }

        [MenuItem("CONTEXT/SkinnedMeshRenderer/Paste BlendShapes Strictly", false, 120)]
        static private void PasteBlendShapeStrictly(MenuCommand cmd) {
            if (_copiedBlendShapes == null) {
                Debug.LogAssertion("PasteBlendShapeStrictly: _copiedBlendShapes is null");
                return;
            }

            SkinnedMeshRenderer smrCmp = cmd.context as SkinnedMeshRenderer;
            List<(string, float)> blendShapes = ExtractBlendShapes(smrCmp);

            if (IsSameBlendShapes(_copiedBlendShapes, blendShapes)) {
                Undo.RecordObject(smrCmp as Object, "Paste BlendShapes Strictly");
                MovBlendShapes(smrCmp);
            } else {
                Debug.Log("PasteBlendShapeStrictly: doesn't equal blendShapes");
            }
        }

        [MenuItem("CONTEXT/SkinnedMeshRenderer/Paste BlendShapes By Name", false, 120)]
        static private void PasteBlendShapeByName(MenuCommand cmd) {
            if (_copiedBlendShapes == null) {
                Debug.LogAssertion("PasteBlendShapeByName: _copiedBlendShapes is null");
                return;
            }

            Undo.RecordObject(cmd.context, "Paste BlendShapes By Name");
            MovBlendShapes(cmd.context as SkinnedMeshRenderer);
        }

        [MenuItem("CONTEXT/SkinnedMeshRenderer/Paste BlendShapes Only Non-Zero Values", false, 120)]
        static private void PasteBlendShapeOnlyNonZero(MenuCommand cmd) {
            if (_copiedBlendShapes == null) {
                Debug.LogAssertion("PasteBlendShapeOnlyNonZero: _copiedBlendShapes is null");
                return;
            }

            SkinnedMeshRenderer smrCmp = cmd.context as SkinnedMeshRenderer;

            Undo.RecordObject(cmd.context, "Paste BlendShapes Only Non-Zero Values");
            MovBlendShapesNonZero(smrCmp);
        }

        // Copy _copiedBlendShapes values to smrCmp
        static private void MovBlendShapes(SkinnedMeshRenderer smrCmp) {
            Mesh mesh = smrCmp.sharedMesh;

            foreach(var (s, f) in _copiedBlendShapes) {
                int idx = mesh.GetBlendShapeIndex(s);

                if (idx != -1) {
                    smrCmp.SetBlendShapeWeight(idx, f);
                }
            }
        }

        // Copy _copiedBlendShapes values to smrCmp, only non-zero values
        static private void MovBlendShapesNonZero(SkinnedMeshRenderer smrCmp) {
            Mesh mesh = smrCmp.sharedMesh;

            foreach(var (s, f) in _copiedBlendShapes) {
                if (f != 0.0f) {
                    int idx = mesh.GetBlendShapeIndex(s);

                    if (idx != -1) {
                        smrCmp.SetBlendShapeWeight(idx, f);
                    }
                }
            }
        }

        // extarct blendshape's name and value and return as list
        static private List<(string, float)> ExtractBlendShapes(SkinnedMeshRenderer smr) {
            List<(string, float)> blendShapes = new List<(string, float)>();
            Mesh mesh = smr.sharedMesh;
            int shape_num = mesh.blendShapeCount;

            for (int i = 0; i < shape_num; i++) {
                blendShapes.Add((mesh.GetBlendShapeName(i), smr.GetBlendShapeWeight(i)));
            }

            return blendShapes;
        }

        // naive implementation
        // compare 2 BlendShape Lists by their name
        // doesn't matter name order
        static private bool IsSameBlendShapes(List<(string, float)> x, List<(string, float)> y) {
            HashSet<string> blendShapeNames_x = new HashSet<string>();
            HashSet<string> blendShapeNames_y = new HashSet<string>();

            x.ForEach(sf => blendShapeNames_x.Add(sf.Item1));
            y.ForEach(sf => blendShapeNames_y.Add(sf.Item1));

            return blendShapeNames_x.SetEquals(blendShapeNames_y);
        }
    }
}