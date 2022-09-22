using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour {
    public Directory entryPoint;

    public DirectoryLine dirLinePrefab;
    public FileLine fileLinePrefab;

    [Serializable] 
    public struct DirectoryPair {
        public DirectoryPair(Directory a, Directory b) {
            this.a = a;
            this.b = b;
        }
        
        public Directory a;
        public Directory b;
    }

    [Serializable]
    public struct FileAssociation {
        public FileAssociation(Directory dir, File file) {
            this.dir = dir;
            this.file = file;
        }

        public Directory dir;
        public File file;
    }

    public DirectoryPair[] directoryEdges;
    public FileAssociation[] directoryFiles;

    private void Start() {
        
#if UNITY_EDITOR
// validate single edges only in editor
        HashSet<DirectoryPair> registeredEdges = new(directoryEdges.Length * 2);
        foreach (DirectoryPair pair in directoryEdges) {
            DirectoryPair reverse = new(pair.b, pair.a);
            if (registeredEdges.Contains(pair) || registeredEdges.Contains(reverse)) {
                Debug.LogError($"Edge [{pair.a.name}, {pair.b.name}] is defined multiple times");
            } else if (pair.a == pair.b) {
                Debug.LogError($"Edge [{pair.a.name}, {pair.b.name}] is a self-reference");
            }
            else {
                registeredEdges.Add(pair);
            }
        }

        HashSet<FileAssociation> fileAssociations = new(directoryFiles.Length);
        foreach (FileAssociation entry in directoryFiles) {
            if (fileAssociations.Contains(entry)) {
                Debug.LogError($"File association [{entry.dir}/{entry.file}] is defined multiple times");
            }
            else {
                fileAssociations.Add(entry);
            }
        }
#endif

        foreach (DirectoryPair pair in directoryEdges) {
            pair.a.connected.Add(pair.b);
            pair.b.connected.Add(pair.a);
            
            DirectoryLine line = Instantiate(dirLinePrefab);
            line.a = pair.a;
            line.b = pair.b;
            line.Setup();
        }

        foreach (FileAssociation assoc in directoryFiles) {
            assoc.dir.files.Add(assoc.file);
            
            FileLine line = Instantiate(fileLinePrefab);
            line.dir = assoc.dir;
            line.file = assoc.file;
            line.Setup();
        }

    }


    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        foreach (DirectoryPair pair in directoryEdges) {
            Gizmos.DrawLine(pair.a.transform.position, pair.b.transform.position); 
        }

        Gizmos.color = Color.cyan;
        foreach (FileAssociation assoc in directoryFiles) {
            Gizmos.DrawLine(assoc.dir.transform.position, assoc.file.transform.position); 
        }

    }
}
