import './style.scss'
import { useEffect, useRef, useState } from 'react'
import { API } from 'src/services'
import { ICaseData } from 'src/interfaces'
import { Button, CaseData, Input, TextArea } from 'src/components'
import Criterias from './criterias'
import Stages from './stages'
import { useNavigate, useParams } from 'react-router-dom'



const CaseUpsertPage = () => {
    const navigate = useNavigate()
    const { id } = useParams()    
    
    const [caseName, setName] = useState<string>('')
    const [caseTask, setTask] = useState<string>('')
    const [caseGuide, setGuide] = useState<string>('')
    const [caseStages, setStages] = useState<string[]>([])
    const [caseCriterias, setCriterias] = useState<string[]>([])

    const preview = useRef<HTMLDialogElement>(null)
    const [previewData, setPreview] = useState<ICaseData>({name: caseName, task: caseTask, video: caseGuide, stages: caseStages, criterias: caseCriterias})

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        const form = e.target as HTMLFormElement
        const formData = new FormData(form)        
        console.debug("formData", formData)
    }

    const showPreview = async() =>  {
        await setPreview({name: caseName, task: caseTask, video: caseGuide, stages: caseStages, criterias: caseCriterias})
        preview.current?.showModal()
    }

    useEffect(() => {        
        if (id == undefined) return
        const data = API.getCase(+id)
        setName(data.name)
        setTask(data.task)
        setGuide(data.video)
        setStages(data.stages)
        setCriterias(data.criterias)
    }, [id])

    return (
        <div className='casePage'>
            <header className='casePage-header'>
                <h1>{id ? 'Обновление направления олимпиады' : 'Создание направления олимпиады'}</h1>
            </header>
            <main className='casePage-main'>
                <form id='upsert-form' onSubmit={(e) => handleSubmit(e)}>
                    <Input defaultValue={caseName} label='Направление' name='case' type='text' onChange={(e) => setName(e.target.value)} />
                    <TextArea defaultValue={caseTask} label='Задание' name='task' onChange={(e) => setTask(e.target.value)}/>
                    <Input label='Мастер-класс' name='video' type='text' defaultValue={caseGuide} onChange={(e) => setGuide(e.target.value)} />
                    <Stages _stages={caseStages} />
                    <Criterias _criterias={caseCriterias} />
                    <div className='btn-container'>
                        <Button type='submit' isActive={true}>Сохранить</Button>
                        <Button type='button' clickHandler={() => showPreview()} isActive={true} variant='passive'>Предпросмотр</Button>
                        <Button type='button' clickHandler={() => navigate('/dashboard/cases')} isActive={true} variant='passive'>Отменить</Button>
                    </div>
                </form>
            </main>
            <dialog id='case-preview' ref={preview}>
                <CaseData userStatus={undefined} caseData={previewData} isPreview={true} />
            </dialog>
        </div>
    )
}
export default CaseUpsertPage