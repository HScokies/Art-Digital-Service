import IProfileStatus, { ICaseData } from "src/interfaces";
import parse from 'html-react-parser';
import { API } from "src/services";
import { useEffect, useState } from "react";
import { FileInput, Button, Stage } from "components/index";

interface props {
    userStatus?: IProfileStatus,
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
            if (!input.files?.length) return setActive(false)
        }
        setActive(true)
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
                    status ?
                        <>
                            <h2 className='profilepage_status-title_results'>Результаты</h2>
                            <p className='profilepage_status-text'>{status.name}</p>
                            {
                                status.file &&
                                <a onClick={() => console.debug("file")} className='profilepage_status-text link'>
                                    Скачать сертификат участника
                                </a>
                            }
                        </>
                        :
                        <>
                            <h2 className='profilepage_status-title'>Отправить задание</h2>
                            <form className='profilepage_status-form' id='task-form'>
                                <FileInput changeHandler={handleFileChange} label='Согласие на обработку персональных данных' required={true} name='consent' accept={['.pdf']} />
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