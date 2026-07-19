# Decompiled Code Tracing

## Start from a verified surface

Begin with the closest observable anchor: UI text, public method, log string, serialized event,
network message, object type, field mutation, or call recorded at runtime. Record why the anchor is
connected to the requested behavior. A text match is a lead, not a call edge.

For each step, record the declaring type, full method signature, assembly, source locator, and file
hash. Preserve overload and generic arguments. When decompiler output is regenerated, use semantic
locators such as type plus signature and a narrow statement description rather than source line
alone.

## Follow every relevant edge kind

Trace direct calls in both directions, then check indirect edges that ordinary text search misses:

- virtual and interface dispatch, including concrete types possible at the call site;
- delegates, C# events, lambdas, closures, and subscription/unsubscription lifetime;
- iterator and async/coroutine entry methods through their generated state-machine `MoveNext`;
- reflection targets and the binding/name/type rules that select them;
- Unity messages such as `Awake`, `OnEnable`, `Start`, `Update`, collision callbacks, and destruction;
- serialized `UnityEvent` persistent calls and animation/event bindings;
- RPC send wrappers, server execution, client receive wrappers, and locally invoked host paths;
- factories, dependency containers, component lookup, and network/object spawning.

Do not stop at a helper name. Follow the path through the statement that reads the decisive value,
consumes RNG, mutates state, sends data, instantiates an object, or formats the visible result.
Include gates, early returns, exception/fallback paths, and cleanup.

## Track values and ownership

For each value in the trace, record:

- original source: argument, field, property, serialized value, singleton, lookup, or return value;
- owning object and lifetime;
- every writer in the declared window;
- whether the value is local, authoritative, mirrored, cached, derived, or presentation-only;
- the exact phase at which a reader observes it.

Do not infer that a field is synchronized because a nearby method is an RPC. Separate method
arguments, pre-call fields, synchronous mutations, server state, receive-stage fields, and later
callbacks. On a host, distinguish local send/server/receive paths even when they run in one process.

## Account for decompiler limits

Record unresolved constructs rather than repairing them silently. Common hazards include optimized
control flow, generated names, missing metadata, stripped methods, invalid stack reconstruction,
compiler-generated state machines, and tool-specific C# that is not the original source.

Use IL or metadata inspection when the C# rendering is ambiguous about overloads, attributes,
branching, literals, or generated wrappers. A byte-string occurrence does not prove an attribute
constructor value or reachable call.

## Prove reachability

A method existing in an assembly is only `present`. To call it `reachable`, establish its caller or
engine/event binding, object construction/load route, lifecycle eligibility, role gate, and relevant
branch predicates. To call it `executed`, obtain runtime evidence or a complete static proof for the
declared deterministic window.

For a negative result, list assemblies, types, signatures, strings, references, event bindings, and
asset roots searched. “Not found” without a bounded search scope is not evidence of absence.
