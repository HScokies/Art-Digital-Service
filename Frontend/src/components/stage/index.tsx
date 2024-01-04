import './style.scss'
import parse from 'html-react-parser';

interface props {
    index: number,
    text: string
}

const Stage = ({ index, text }: props) => {

    return (
        <div className="stage">
            <span className='stage-index'>{index}</span>
            <span className='stage-descr'>{parse(text)}</span>
        </div>
    )
}
export default Stage