using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Reflection : MonoBehaviour
{
    [ContextMenu("Components")]
    void PRINT_GAMEOBJECT_COMPONENTS(){
        List<Component> components = gameObject.GetComponents<Component>().ToList();
        foreach(var item in components){
            Debug.Log($"Component {item} ComponentName : {item.name}, ComponentTag {item.tag}");
        }
    }
    [ContextMenu("Parent Component")]
    void PRINT_GAMEOBJECT_PARENTCOMPONENT(){
        List<Component> components = gameObject.GetComponentsInParent<Component>().ToList();
        foreach(var parItem in components){
            Debug.Log($"Component {parItem} ComponentName : {parItem.name}, ComponentTag {parItem.tag}");
        }
    }
    [ContextMenu("Child Component")]
    void PRINT_GAMEOBJECT_CHILDCOMPONENT(){
        List<Component> components = gameObject.GetComponentsInChildren<Component>().ToList();
        foreach(var childsItem in components){
            Debug.Log($"Component {childsItem} ComponentName : {childsItem.name}, ComponentTag {childsItem.tag}");
        }
    }

    [ContextMenu("Properties")]
    void PRINT_CLASS_PROPERTIES(){
        var propertiesList = typeof(Equipment_003).GetProperties().ToList();
        foreach(var item in propertiesList){
            Debug.Log($"PropertyInfo : {item}, Name {item.Name}");
        }
    }

    [ContextMenu("Base Class")]
    void PRINT_CLASS_BASE(){
        var baseClass = typeof(Equipment_003).BaseType;
            Debug.Log($"Type : {baseClass}, Name {baseClass.Name}, FullName : {baseClass.FullName}");
    }

    [ContextMenu("Interface")]
    void PRINT_CLASS_INTERFACE(){
        var interfacesList = typeof(Equipment_003).GetInterfaces().ToList();
        foreach(var item in interfacesList){
            Debug.Log($"Type : {item}, Name {item.Name}, FullName : {item.FullName}");
        }
    }

    [ContextMenu("Enum")]
    void PRINT_CLASS_ENUM(){
        var enums_name = System.Enum.GetNames(typeof(E_DebuffState)).ToList();
        var enums_value = System.Enum.GetValues(typeof(E_DebuffState)).OfType<int>().ToList();
        var enums_value_string = System.Enum.GetValues(typeof(E_DebuffState)).OfType<string>().ToList();

        Debug.Log("===============================================");
        Debug.Log("enums_name");
        Debug.Log("===============================================");
        foreach(var item in enums_name){ Debug.Log(item); }
        Debug.Log("===============================================");
        Debug.Log("enums_value");
        Debug.Log("===============================================");
        foreach(var item in enums_value){ Debug.Log(item); }
        Debug.Log("===============================================");
        Debug.Log("enums_value_string");
        Debug.Log("===============================================");
        foreach(var item in enums_value_string){ Debug.Log(item); }
    }
}