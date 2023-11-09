using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public class DirectedAcyclicGraph<T> {
        private Dictionary<T, List<T>> _graph = new Dictionary<T, List<T>>();
        private Dictionary<T, int> _inDegree = new Dictionary<T, int>();

        public void AddEdge(T from, T to) {
            if (!_graph.ContainsKey(from)) {
                _graph[from] = new List<T>();
            }
            if (!_graph.ContainsKey(to)) {
                _graph[to] = new List<T>();
            }
            _graph[from].Add(to);
            if (!_inDegree.ContainsKey(from)) {
                _inDegree[from] = 0;
            }
            if (!_inDegree.ContainsKey(to)) {
                _inDegree[to] = 0;
            }
            _inDegree[to]++;
        }

        public void RemoveEdge(T from, T to) {
            if (!_graph.ContainsKey(from)) {
                return;
            }
            if (!_graph.ContainsKey(to)) {
                return;
            }
            _graph[from].Remove(to);
            _inDegree[to]--;
        }

        public void RemoveNode(T node) {
            if (!_graph.ContainsKey(node)) {
                return;
            }
            foreach (var to in _graph[node]) {
                _inDegree[to]--;
            }
            _graph.Remove(node);
            _inDegree.Remove(node);
        }

        public List<T> TopologicalSort() {
            var result = new List<T>();
            var queue = new Queue<T>();
            foreach (var node in _graph.Keys) {
                if (_inDegree[node] == 0) {
                    queue.Enqueue(node);
                }
            }
            while (queue.Count > 0) {
                var node = queue.Dequeue();
                result.Add(node);
                foreach (var to in _graph[node]) {
                    _inDegree[to]--;
                    if (_inDegree[to] == 0) {
                        queue.Enqueue(to);
                    }
                }
            }
            return result;
        }
    }
}
