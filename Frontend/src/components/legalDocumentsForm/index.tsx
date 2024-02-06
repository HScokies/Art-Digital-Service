import { API } from "src/services"
import { FileInput } from ".."


const LegalDocumentsForm = () => {
    const onSubmit = async(e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        await API.appendLegalFiles(new FormData(e.target as HTMLFormElement))
    }
    return(
        <form id="update-legal" onSubmit={(e) => onSubmit(e)}>
            <FileInput name="regulations" label="Положение об Олимпиаде" accept={[".pdf"]} downloadLink={API.URL+"files/legal/regulations.pdf?displayedName=Положение об Олимпиаде"} />
            <FileInput name="privacyPolicy" label="Политика конфиденциальности" accept={[".pdf"]} downloadLink={API.URL+"files/legal/privacy_policy.pdf?displayedName=Политика конфиденциальности"} />
            <FileInput name="adultConsent" label="Согласие на обработку персональных данных (взрослые)" accept={[".pdf"]} downloadLink={API.URL+"files/legal/a_consent.pdf?displayedName=Согласие на обработку персональных данных"} />
            <FileInput name="youthConsent" label="Согласие на обработку персональных данных (дети)" accept={[".pdf"]} downloadLink={API.URL+"files/legal/y_consent.pdf?displayedName=Согласие на обработку персональных данных"} />
        </form>
    )
}
export default LegalDocumentsForm