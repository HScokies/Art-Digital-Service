import './style.scss'
import Icons from 'images/icons.svg'
import { useEffect, useState } from 'react'
import { Button, TextArea } from 'src/components'

interface props {
    _stages?: string[]
}
const Stages = ({ _stages = [] }: props) => {
    const [stages, setStages] = useState(_stages)

    useEffect(() => {
        setStages(_stages)
    }, [_stages])

    const addStage = async () => {
        let copy = stages
        await setStages([])
        copy.push('')
        setStages(_stages)
    }

    const deleteStage = async(index: number) => {
        let copy = stages
        await setStages([])
        copy.splice(index, 1)
        setStages(copy)
    }

    const setStage = async(index: number, text: string = '') => {
        setStages((stages) => {
            stages[index] = text
            return stages
        })    
    }
    return (
        <>
            <div className='upsert-form-multiple header'>
                <h2>Порядок выполнения</h2>
                <Button isActive={true} type='button' clickHandler={addStage}>Добавить</Button>
            </div>
            <div className='upsert-form-multiple'>
                {
                    stages.map((s, i) => (
                        <div key={i} className='upsert-form-multiple_element'>
                            <TextArea key={i} onBlur={(e) => setStage(i, e)} label={`Этап №${i + 1}`} name='stage' defaultValue={s} />
                            <svg onClick={() => deleteStage(i)}>
                                <use xlinkHref={Icons + '#trash'} />
                            </svg>
                        </div>
                    ))
                }
            </div>
        </>
    )
}
export default Stages