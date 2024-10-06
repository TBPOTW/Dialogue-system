# Dialogue-system
Диалоговая система с ключами на Unity

## КАК ИСПОЛЬЗОВАТЬ?
1. Создаете пустой объект и добавляете скрипт "DialogueManager".
2. Нажимаете на плюсик и у вас появиться 2 поля, ключ и объект.
3. В поле ключ пишем строку, по которой будете обращаться для начала диалога.
4. Создаете новый объект с которым будет происходить диалог и добавляете скрипт "Dialogue_".
5. В поле объекта оставляете ссылку на этот объект.
6. Готово! 
### как обращаться к заданию через код?
1. Обращаетесь к скрипту через Instance
2. Вызываете метод `StartDialogueByKey`
#### Пример
`DialogueManager.Instance.StartDialogueByKey(key)`
## КАК ЭТО РАБОТАЕТ?
#### Иницилизация базы данных
В самом начале иницилизируем базу данных (все диалоги, которые вы ввели в инспекторе)
```C#
private void InitializeDialogueDataBase()
{
    // Перед заполнением на всякий случай очищаем нашу базу данных
    dialogueDataBase.Clear();

    // Заполняем dialogueDataBase ключами и значениями которые мы укажем в листе dialogues
    for (int i = 0; i < dialogues.Count; i++)
    {
        dialogueDataBase.Add(dialogues[i].key, dialogues[i].dialogueObj);
    }
}
```
#### QuestManager - начать диалог
1. Проверяем есть ли диалог с ключом key в базе данных
2. Если его нет - выводим соответсвующее сообщение
3. Иначе обращаемся к компоненту `Dialogue_` у объекта с диалогом и начинаем диалог 
```C#
public void StartDialogueByKey(string key)
{
    if (!dialogueDataBase.ContainsKey(key))
    {
        Debug.LogError($"диалога c ключом \"{key}\" нету в dialogueDataBase");
        return;
    }
    activeDialogue = dialogueDataBase[key];
    // запускаем с нашим ключом
    dialogueDataBase[key].GetComponent<Dialogue_>().StartDialogue();
}
```
#### Dialogue_ - начать диалог
1. Активируем окно диалога.
Если предложение еще не печатается:
    * Печатаем предложение
    * Воспроизводим звук
    * Увеличиваем счетчик, чтобы напечатать следующее предложение
Если все предложения напечатаны, то заканчиваем диалог
```C#
public void StartDialogue()
{
    dialogueBox.SetActive(true);
    if (i + 1 <= sentences.Length)
    {
        if (!isTypingSentence)
        {
            StartCoroutine(TypeSentence(sentences[i]));
            isTypingSentence = true;
            playSound(sounds[0], volume: 0.5f, p1: 1f, p2: 1f);
            i++;
        }
    }
    else
    {
        EndDialogue();
        i = 0;
    }
}
```
#### QuestManaget - закончить диалог
Если у нас воспроизводится диалог: 
  1. Заканчиваем его обращаясь к компоненту `Dialogue_` объекта, с которым сейчас разговариваем
  2. Говорим, что `activeDialogue = null` 
```C#
public void EndDialogueByKey()
{
    if (activeDialogue != null)
    {
        activeDialogue.gameObject.GetComponent<Dialogue_>().EndDialogue();
        activeDialogue = null;
    }
}
```
#### Dialogue_ - закончить диалог
1. Скрываем курсор
2. Скрываем окно диалога
```C#
public void EndDialogue()
{
    Cursor.lockState = CursorLockMode.Locked;
    dialogueBox.SetActive(false);
}
```
#### Dialogue_ - напечатать предложение диалога
1. Проходимся посимвольно по всей встроке
2. Добавяем в пустую строку 1 символ и ждем 0.01 секунду, чтобы получилась анимация появления букв
```C#
IEnumerator TypeSentence(string sentence)
{
    if (name_speaker != "")
        dialogueText.text = name_speaker + ": ";
    else
        dialogueText.text = "";
    foreach (char letter in sentence.ToCharArray())
    {
        dialogueText.text += letter;
        yield return new WaitForSeconds(0.01f);
    }
    isTypingSentence = false;
}
```
