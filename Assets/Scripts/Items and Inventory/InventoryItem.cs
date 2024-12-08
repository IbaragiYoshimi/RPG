using System;

/* ע�� ItemObjec ���� InventoryItem ��Ĳ�ͬ��ǰ���ǹ����� GameObject �ϵģ��̳��� Monobehaviour�����ڼ����ײ��
 * �������ǿ��������ڰ�װ�ɿ����������͵� ItemData����Ҫ��ά�������������Ϣ�����ں�̨������ʹ�ã������������ Scene �С��ʲ���Ҫ�̳� Monobehaviour�� */
[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize = 0;

    // ʵ����ʱ���ض�����һ��������
    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
