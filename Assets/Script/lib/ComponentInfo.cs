using UnityEngine;
using System.Collections;

/// instance has a componentName and attachedObjName
public class ComponentInfo {
	public string attachedObjName;
	public string componentName;

	public ComponentInfo(string attachedObjName, string componentName) {
		this.attachedObjName = attachedObjName;
		this.componentName = componentName;
	}

	override
	public string ToString() {
		return attachedObjName + "#" + componentName;
	}

	public Component GetComponent() {
		return GameObject.Find(attachedObjName).GetComponent(componentName);
	}
}
