export const PhoneChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const phone = e.target.value
    let startAdding = false
    let newPhone = "+7 "

    for (let i = 0; i < phone.length; i += 1) {
        if (phone[i] == '+' && phone[i + 1] == '7' && phone[i + 2] == ' ') {
            startAdding = true;
            i += 2;
            continue;
        }

        if (!startAdding)
            continue;

        if (!isNaN(+phone[i]) && phone[i] != ' ')
            newPhone += phone[i];
    }
    e.target.value = newPhone;
}
