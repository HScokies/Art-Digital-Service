import { ICaseData, IStatus } from "src/interfaces";
import parse from 'html-react-parser';
import { useEffect, useState } from "react";
import { FileInput, Button, Stage } from "components/index";
import { API } from "src/services";

interface props {
    userStatus?: IStatus,
    caseData: ICaseData,
    isPreview?: boolean
}
const CaseData = ({ userStatus, caseData, isPreview = false }: props) => {
    const [status, setStatus] = useState(userStatus)
    const [caseInfo, setCase] = useState(caseData)
    useEffect(() => {
        setStatus(userStatus)
        setCase(caseData)
    }, [userStatus, caseData])

    const [buttonActive, setActive] = useState(false)
    const handleFileChange = () => {
        const inputs = document.getElementsByClassName("fileinput_wrapper-field")
        for (let i = 0; i < inputs.length; i++) {
            const input = inputs[i] as HTMLInputElement
            if (!input.files?.length || !validateFileSize(input.files[0])) return setActive(false)
        }
        setActive(true)
    }

    const validateFileSize = (file: Blob) => {
        const megaByte = 1_048_576;
        return file.size < (megaByte * 3)
    }

    const onSubmit = async(e : React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        const response = await API.appendFiles(new FormData(e.target as HTMLFormElement))
        if (response.status != 200) return;
        setStatus({
            text: "Ваша заявки принята и находится на рассмотрении.\nОжидайте результатов!",
            download: false
        });
    }



    return (
        <>
            <section className='profilepage_task'>
                <h2 className='profilepage_task-title'>
                    Задание
                </h2>
                <p className='profilepage_task-descr'>
                    {parse(caseInfo.task)}
                </p>
            </section>
            <section className='profilepage_status'>
                {
                    status != null ?
                        <>
                            <h2 className='profilepage_status-title_results'>Результаты</h2>
                            <p className='profilepage_status-text'>{status.text}</p>
                            {
                                status.download &&
                                <a href="#" className='profilepage_status-text link'>
                                    Скачать сертификат участника
                                </a>
                            }
                        </>
                        :
                        <>
                            <h2 className='profilepage_status-title'>Отправить задание</h2>
                            <form className='profilepage_status-form' id='task-form' onSubmit={(e) => onSubmit(e)}>
                                <FileInput changeHandler={handleFileChange} label='Согласие на обработку персональных данных' required={true} name='conscent' accept={['.pdf', '.jpg', '.bmp', '.png']} />
                                <FileInput changeHandler={handleFileChange} label='Выполненное задание' name='solution' required={true} accept={['.pdf']} />
                                <Button isActive={buttonActive && !isPreview}>
                                    Отправить
                                </Button>
                            </form>
                        </>

                }
            </section>
            <section className='profilepage_guide'>
                <h2 className='profilepage_guide-title'>
                    Мастер-класс
                </h2>
                <iframe className='profilepage_guide-video' frameBorder='0' allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture' allowFullScreen={true} src={`https://www.youtube.com/embed/${caseData.youtubeId}`}></iframe>
            </section>
            <section className='profilepage_stages'>
                <h2 className='profilepage_stages-title'>
                    Порядок выполнения
                </h2>
                {
                    caseInfo.stages.map((e, i) => (
                        <Stage key={i} index={i + 1} text={e} />
                    ))
                }
            </section>
            <section className='profilepage_criterias'>
                <h2 className='profilepage_criterias-title'>
                    Критерии
                </h2>
                <div className='profilepage_criterias-container'>
                    {
                        caseInfo.criterias.map((e, i) => (
                            <span key={i} className='profilepage_criterias-container-criteria'>{parse(e)}</span>
                        ))
                    }
                </div>
            </section>
        </>
    )
}
export default CaseData