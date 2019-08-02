import React, { Component } from 'react'
import { Container, Row, Col } from 'reactstrap';
import Note from './Note'
import NoteInput from './NoteInput'
import { getNotes, createNote } from './Fetch'
import ErrorPage from './ErrorPage'
import '../styles/styles.css'

export default class Notes extends Component {
    constructor (props) {
        super(props);
        this.state = { notes: [], loading: true, err: false};

        this.last = React.createRef();
    }

    renderNotes = (notes) => {
        return (
            <div>
                {notes.map(n => (
                    <Note 
                        key={n.id} 
                        id={n.id} 
                        text={n.text}
                        date={n.dateModified}/>
                ))}
            </div>
        )
    }

    createNew = async (text) => {
        await createNote(text);
        await this.fetchNotes();
    }

    fetchNotes = async () => {
        const {obj, status} = await getNotes();
        if (status === 200)
            this.setState({notes: obj, loading: false})
        if (status === 500)
            this.setState({err: true})
    }

    async componentDidMount() {
        await this.fetchNotes();
        if (!this.state.err)
            this.last.current.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            })
    }

    componentDidUpdate() {
        if (!this.state.err)
            this.last.current.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            })
    }

    render(){
        let page = <ErrorPage/>
        if (!this.state.err) {
            const contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderNotes(this.state.notes)

            page = 
                <Container ref={this.cont}>  
                    <Row>   
                        <Col className="note-container">
                            {contents}
                            <div ref={this.last}></div>
                        </Col>
                    </Row>
                    <Row>                    
                        <NoteInput create={this.createNew}/>                    
                    </Row>
                </Container>
        }

        return (      
            page
        );
    }
}