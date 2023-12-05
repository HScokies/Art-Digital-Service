export class Validator{
    static validateEmail = (email: string): string => {
        const isValid = String(email)
        .toLowerCase()
        .match(
          /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
        );
        if (!isValid)
            return "Неверный адрес электронной почты"
        return ""
    }

    static validatePassword = (password: string): string => {
        const numberRegEx = /\d/
        const letterRegEx = /[a-zA-Z]/g

        if (password.length < 7)
            return "Слишком короткий"
        if (password.indexOf(" ") != -1)
            return "Не должен содержать пробелы";
        if (!numberRegEx.test(password))
            return "Должен содержать цифры"
        if (!letterRegEx.test(password))
            return "Должен содержать латинские буквы"
        return "";
    }
}