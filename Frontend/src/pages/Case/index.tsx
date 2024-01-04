import { useParams } from 'react-router-dom'
import './style.scss'
import { useEffect, useState } from 'react'
import { API } from 'src/services'
import { ICaseData } from 'src/interfaces'
import { Button, Input, TextArea } from 'src/components'
import Icons from 'images/icons.svg'

const CasePage = () => {
    const { id } = useParams()
    const [data, setData] = useState<ICaseData | undefined>({
        name: '',
        task: '',
        video: '',
        stages: [],
        criterias: []
    })

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        console.debug(new FormData(e.target))
    }

    useEffect(() => {
        if (!id) return
        const data = API.getCase(+id)
        setData(data)
    }, [id])

    const addStage = async () => {
        let oldData = data
        await setData(undefined)
        oldData?.stages.push('')
        setData(oldData)
    }

    const deleteStage = async(index: number) => {
        let oldData = data
        await setData(undefined)
        oldData?.stages.splice(index, 1)
        setData(oldData)
    }

    const setStage = async(index: number, text: string = '') => {
        setData((oldData) => {
            if (oldData)
                oldData.stages[index] = text
            return oldData
        })    
    }

    return (
        <section className='casePage'>
            <header className='casePage-header'>
                <h1>{id ? 'Обновление направления олимпиады' : 'Создание направления олимпиады'}</h1>
            </header>
            <main className='casePage-main'>
                <form id='upsert-form' onSubmit={(e) => handleSubmit(e)}>
                    <Input defaultValue={data?.name} label='Направление' name='case' type='text' />
                    <TextArea defaultValue={data?.task} label='Задание' name='task' />
                    <Input label='Мастер-класс' name='video' type='text' />
                    <div className='upsert-form-stages header'>
                        <h2>Порядок выполнения</h2>
                        <Button isActive={true} type='button' clickHandler={addStage}>Добавить</Button>
                    </div>
                    <div className='upsert-form-stages'>
                        {
                            data &&
                            data.stages.map((s, i) =>
                                <div key={i} className='upsert-form-stages_stage'>
                                    <TextArea key={i} onBlur={(e) => setStage(i, e)} label={`Этап №${i + 1}`} name='stage' defaultValue={s} />
                                    <svg onClick={() => deleteStage(i)}>
                                        <use xlinkHref={Icons + '#trash'} />
                                    </svg>
                                </div>
                            )
                        }
                    </div>
                    <Button isActive={true}>Submit</Button>
                </form>
            </main>
        </section>
    )
}
export default CasePage