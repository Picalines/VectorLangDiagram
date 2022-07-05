# VectorLangDiagram

Это была курсовая работа. Основная суть:
 * В одном C# проекте написан *очень простенький* ""компилятор"" собственного языка программирования, который предназначен для отрисовки векторных диаграмм.
 * В другом проекте небольшое приложение на Blazor (запуск через electron), в котором можно редактировать и запускать (т.е. просматривать построенные диаграммы) программы на языке из первого проекта.

Для Blazor приложения есть небольшая часть на TypeScript для подключения пары библиотек. ~~Осторожно, пахнет клеем~~

В папке [ExamplePrograms](ExamplePrograms) лежат, не поверите, примеры программ на языке *VectorLang*.

Такая программка нарисует рекурсивное бинарное дерево:
```
external branchLengthMult = 0.84;
external minBranchLength = 0.2;
external branchAngle = 30;

def branch(number length) -> void = if (length >= minBranchLength) [
    val v := {0, length};
    plot(v);
    translate(v);

    length := length * branchLengthMult;

    val angleRad := branchAngle.degToRad();

    push();
    rotate(angleRad);
    branch(length);
    pop();

    push();
    rotate(-angleRad);
    branch(length);
    pop();
];

def main() -> void = [
    branch(1);
];
```

![image](https://user-images.githubusercontent.com/42614422/177333214-20991a47-d45f-42f0-9d3f-026c056b7ca1.png)
