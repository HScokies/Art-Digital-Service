import './style.scss'
import { useEffect, useRef, useState } from 'react'
import { API } from 'src/services'
import { ICaseData } from 'src/interfaces'
import { Button, CaseData, Input, TextArea } from 'src/components'
import Criterias from './criterias'
import Stages from './stages'
import { useNavigate, useParams } from 'react-router-dom'
import { Pages } from 'src/enums'



const CaseUpsertPage = () => {
    const navigate = useNavigate()
    const { id } = useParams()

    const [caseName, setName] = useState<string>('')
    const [caseTask, setTask] = useState<string>('')
    const [youtubeId, setId] = useState<string>('')
    const [caseStages, setStages] = useState<string[]>([])
    const [caseCriterias, setCriterias] = useState<string[]>([])

    const preview = useRef<HTMLDialogElement>(null)
    const [previewData, setPreview] = useState<ICaseData>({ name: caseName, task: caseTask, youtubeId: youtubeId, stages: caseStages, criterias: caseCriterias })

    const handleSubmit = async(e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        const form = e.target as HTMLFormElement
        const formData = new FormData(form)

        if (id == undefined || isNaN(+id)) return await API.createCase(formData)
        return await API.updateCase(+id, formData);
    }

    const showPreview = async () => {
        await setPreview({ name: caseName, task: caseTask, youtubeId: youtubeId, stages: caseStages, criterias: caseCriterias })
        preview.current?.showModal()
    }

    useEffect(() => {
        if (id == undefined) return
        const fetch = async () => {
            const response = await API.getCase(+id)
            if (response.status != 200) return;
            const data = response.data as ICaseData;
            setName(data.name)
            setTask(data.task)
            setId(data.youtubeId)
            setStages(data.stages)
            setCriterias(data.criterias)
        }
        fetch();
    }, [id])

    return (
        <div className='casePage'>
            <header className='casePage-header'>
                <h1>{id ? 'Обновление направления олимпиады' : 'Создание направления олимпиады'}</h1>
            </header>
            <main className='casePage-main'>
                <form id='upsert-form' onSubmit={(e) => handleSubmit(e)}>
                    <Input defaultValue={caseName} label='Направление' name='name' type='text' onChange={(e) => setName(e.target.value)} />
                    <TextArea defaultValue={caseTask} label='Задание' name='task' onChange={(e) => setTask(e.target.value)} />
                    <Input label='Мастер-класс (id youtube видео)' name='youtubeId' type='text' defaultValue={youtubeId} onChange={(e) => setId(e.target.value)} />
                    <Stages _stages={caseStages} />
                    <Criterias _criterias={caseCriterias} />
                    <div className='btn-container'>
                        <Button type='submit' isActive={true}>Сохранить</Button>
                        <Button type='button' clickHandler={() => showPreview()} isActive={true} variant='passive'>Предпросмотр</Button>
                        <Button type='button' clickHandler={() => navigate(Pages.dashboard+'cases')} isActive={true} variant='passive'>Отменить</Button>
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