# Serialized Asset Resolution

## Preserve source identity

Record the original bundle/asset/scene source, its hash, extraction tool/version, exported relative
path, Unity class/type, GUID, fileID/path ID, and owning asset context. Exported filenames and object
names are navigation aids, not stable identities.

Join a `MonoBehaviour` to its `MonoScript` and managed class through the serialized script reference
and `.meta` GUID when available. Confirm assembly, namespace, and type rather than assuming the
filename matches the class. Resolve other references by GUID/fileID or bundle/path ID according to
the evidence format.

## Traverse the dependency graph

Search build scene lists, scenes, prefabs, nested prefabs, prefab variants, ScriptableObjects,
Resources, Addressables catalogs, asset bundles, and referenced materials/animations when relevant.
Follow dependencies to the concrete loaded object; do not stop at the first same-named asset.

For prefab composition, retain owner-qualified identities:

- property modification: owner instance/asset, target correspondence, property path;
- added object/component: owner plus owner-local added identity and attachment target;
- removed object/component: owner plus removed-source correspondence;
- stripped proxy: owning prefab instance, stripped local identity, and source correspondence.

Names and local fileIDs can repeat across nested instances. Resolve correspondence in the context
of the owning prefab instance and source asset.

## Compose the effective serialized state

Apply the relevant layers in Unity precedence order:

1. managed field initializer or constructor-created default;
2. base prefab serialized fields and object graph;
3. nested prefab and variant modifications, additions, removals, and stripped proxies;
4. scene prefab-instance modifications and scene-owned components;
5. runtime instantiation parameters, `Awake`/`OnEnable`/`Start`, later assignments, and network data.

Confirm which GameObjects and components survive composition before resolving their properties.
Record every contributing layer, including an explicit absence of an override. Do not call a code
initializer the runtime default when serialization replaces it.

## Separate value resolution from applicability

An effective serialized value may exist on an inactive object, disabled component, unloaded scene,
unselected variant, or unreachable bundle. Record separately:

- structural existence in the composed object graph;
- active/enabled state where it affects the behavior;
- scene or asset load path;
- runtime instantiation and replacement;
- whether the object/value can affect the declared observation window.

A disabled component can still be read by other code, so disabled does not automatically mean
irrelevant. Conversely, a fully resolved prefab that is never loaded does not establish runtime use.

## Treat special serialized behavior explicitly

Trace persistent `UnityEvent` targets, methods, argument modes, and owning object instances. Inspect
animation clips/events, state-machine behaviors, curves, and declarative bindings when they invoke
or override the code path. For an `AnimationCurve`, preserve keys, tangents, weights, wrap modes,
input precision, and the consuming cast/clamp; a custom evaluator is not a Unity-runtime oracle by
itself.

When an exporter omits unsupported fields, managed references, type trees, or bundle metadata,
record the extraction gap and inspect another representation or leave the result unknown. Do not
convert an exporter limitation into evidence that the source value is absent.
